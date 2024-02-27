namespace Minitwit.Core;

public interface IUserRepository
{   
    Task<int> GetUserId(string username);
    string GetGravatarUrl(string email, int size = 80);
    Task<UserLoginDto> GetUserForLogin(string username);
    Task CreateUser(string username, string password, string email);
    Task<string> GetUsername(int user_id);
}