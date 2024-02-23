namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Chirp.Core;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpContext _context;

    public AuthorRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task CreateAuthor(CreateAuthorDto author)
    {
        if(await AuthorExists(author.Name)){
            throw new Exception("Author already exists");
        }
        var newAuthor = new Author
        {
            AuthorId = Guid.NewGuid(),
            Name = author.Name,
            Email = author.Email,
            Cheeps = new List<Cheep>()
        };

        await _context.Authors.AddAsync(newAuthor);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AuthorExists(string name){
        return await _context.Authors.AnyAsync(a => a.Name == name);
    }

    public async Task FollowAuthor(string authorName, string authorToFollowName) {
        var author = await _context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        var authorToFollow = await _context.Authors
            .Include(a => a.Followers)
            .FirstOrDefaultAsync(a => a.Name == authorToFollowName);

        if (author == null || authorToFollow == null) {
            throw new Exception("Author doesn't exist");
        }

        // Check if author is already following authorToFollow
        // This should be unnecessary as "Following" and "Followers" should be Sets
        // Additionally the database should prevent duplicates as it has a primary key on both columns(It does not, see src/Chirp.Infrastructure/Migrations/20231120134820_AddFollowersToAuthor.cs))
        if (author.Following.Contains(authorToFollow) || authorToFollow.Followers.Contains(author)) {
            throw new Exception("Duplicate follow");
        }

        author.Following.Add(authorToFollow);
        authorToFollow.Followers.Add(author);

        await _context.SaveChangesAsync();
    }

    public async Task UnfollowAuthor(string authorName, string authorToUnfollowName) {
        var author = await _context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        var authorToUnfollow = await _context.Authors
            .Include(a => a.Followers)
            .FirstOrDefaultAsync(a => a.Name == authorToUnfollowName);

        if (author == null || authorToUnfollow == null) {
            throw new Exception("Author doesn't exist");
        }

        author.Following.Remove(authorToUnfollow);
        authorToUnfollow.Followers.Remove(author);

        await _context.SaveChangesAsync();
    }

    public IEnumerable<string> GetAuthorFollowing(string authorName) {
        var author = _context.Authors
            .Where(a => a.Name == authorName)
            .SelectMany(a => a.Following)
            .Select(a => a.Name);

        return author;
    }

    public IEnumerable<string> GetAuthorFollowers(string authorName) {
        var author = _context.Authors
            .Where(a => a.Name == authorName)
            .SelectMany(a => a.Followers)
            .Select(a => a.Name);

        return author;
    }

    public async Task DeleteAuthor(string authorName)
    {
        var author = await _context.Authors
            .Include(a => a.Cheeps)
            .Include(a => a.Followers)
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Name == authorName) ?? throw new Exception("Author doesn't exist");
            
		_context.Cheeps.RemoveRange(author.Cheeps);

        foreach (var follower in author.Followers)
        {
            follower.Following.Remove(author);
        }
        foreach (var following in author.Following)
        {
            following.Followers.Remove(author);
        }

        _context.Authors.Remove(author);
        
        await _context.SaveChangesAsync();
    }
}
