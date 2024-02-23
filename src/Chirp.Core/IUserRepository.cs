namespace Chirp.Core;

public interface IUserRepository
{   
    Task<int> GetUserId(string username);
    string GetGravatarUrl(string email, int size = 80);
}
    /* PLEASE DELETE SOON 
    IEnumerable<string> GetAuthorFollowers(string authorName);
    IEnumerable<string> GetAuthorFollowing(string authorName);
    Task<bool> AuthorExists(string authorName);
    Task CreateAuthor(CreateAuthorDto author);
    Task DeleteAuthor(string authorName);
    Task FollowAuthor(string authorName, string authorToFollowName);
    Task UnfollowAuthor(string authorName, string authorToUnfollowName);
}
    */