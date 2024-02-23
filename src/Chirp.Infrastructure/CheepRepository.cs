namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using FluentValidation;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;
    private readonly CreateCheepValidator _validator;

    public CheepRepository(ChirpContext context, CreateCheepValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IEnumerable<CheepDto>> GetCheeps(int pageIndex, int pageRange)
    {
        return await _context.Cheeps
            .Include(c => c.Author)
            .OrderByDescending(c => c.TimeStamp)
            .Skip((pageIndex - 1) * pageRange)
            .Take(pageRange)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp, c.CheepId))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string authorName, int pageIndex, int pageRange)
    {
        return await _context.Cheeps
            .Include(c => c.Author)
            .OrderByDescending(c => c.TimeStamp)
            .Where(c => c.Author.Name == authorName)
            .Skip((pageIndex - 1) * pageRange)
            .Take(pageRange)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp, c.CheepId))
            .ToListAsync();
    }

    public async Task<IEnumerable<CheepDto>> GetPersonalCheeps(string authorName, int pageIndex, int pageRange)
    {
        var following = await _context.Authors
            .Include(a => a.Following)
            .Where(a => a.Name == authorName)
            .SelectMany(a => a.Following)
            .ToListAsync();

        return await _context.Cheeps
            .Include(c => c.Author)
            .OrderByDescending(c => c.TimeStamp)
            .Where(c => c.Author.Name == authorName || following.Contains(c.Author))
            .Skip((pageIndex - 1) * pageRange)
            .Take(pageRange)
            .Select(c => new CheepDto(c.Text, c.Author.Name, c.TimeStamp, c.CheepId))
            .ToListAsync();
    }
    
    public async Task CreateCheep(CreateCheepDto cheep)
    {
        var validationResult = await _validator.ValidateAsync(cheep);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == cheep.AuthorName);

        var newCheep = new Cheep
        {
            CheepId = Guid.NewGuid(),
            AuthorId = author!.AuthorId,
            Author = author,
            Text = cheep.Text,
            TimeStamp = DateTime.Now
        };

        await _context.Cheeps.AddAsync(newCheep);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCheep(Guid cheepId)
    {
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);
        _context.Cheeps.Remove(cheep!);
        await _context.SaveChangesAsync();
    }
}