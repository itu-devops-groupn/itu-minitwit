namespace Minitwit.Infrastructure;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

using Minitwit.Core;

public class UserRepository : IUserRepository
{
    private readonly MinitwitContext _context;

    public UserRepository(MinitwitContext context)
    {
        _context = context;
    }

    public string GetGravatarUrl(string username, int size = 80)
    {
        var hash = MD5.HashData(Encoding.UTF8.GetBytes(username));
        var builder = new StringBuilder();
        foreach (var b in hash)
        {
            builder.Append(b.ToString("x2"));
        }
        return $"https://www.gravatar.com/avatar/{builder}?d=identicon&s={size}";
    }

    public Task<int> GetUserId(string username)
    {
        var user = _context.Users
            .Where(u => u.Username == username)
            .Select(u => u.User_id)
            .FirstOrDefaultAsync();

        return user;
    }

    public Task<UserLoginDto> GetUserForLogin(string username)
    {
        var user = _context.Users
            .Where(u => u.Username == username)
            .Select(u => new UserLoginDto(u.Username, u.Pw_hash))
            .FirstOrDefaultAsync();

        return user;
    }

    public Task CreateUser(string username, string password, string email)
    {
        var user = new User
        {
            Username = username,
            Pw_hash = password,
            Email = email
        };

        _context.Users.Add(user);
        return _context.SaveChangesAsync();
    }

    public Task<string> GetUsername(int user_id)
    {
        var username = _context.Users
            .Where(u => u.User_id == user_id)
            .Select(u => u.Username)
            .FirstOrDefaultAsync();

        return username;
    }
}