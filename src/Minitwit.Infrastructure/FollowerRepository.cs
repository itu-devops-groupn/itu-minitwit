namespace Minitwit.Infrastructure;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

using Minitwit.Core;

public class FollowerRepository : IFollowerRepository
{

    private readonly MinitwitContext _context;

    public FollowerRepository(MinitwitContext context)
    {
        _context = context;
    }
    public async Task CreateFollower(int who_id, int whom_id)
    {
        var newFollower = new Follower 
        {
            Who_id = who_id,
            Whom_id = whom_id
        };

        await _context.Followers.AddAsync(newFollower);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFollower(int who_id, int whom_id)
    {
        var followerToRemove = await
            _context.Followers
                .FirstOrDefaultAsync(f => f.Who_id == who_id && f.Whom_id == whom_id);
        
        _context.Followers.Remove(followerToRemove!);
        await _context.SaveChangesAsync();

    }

    public Task<IEnumerable<int>> GetFollowers(int user_id)
    {
        var followers = _context.Followers
            .Where(f => f.Whom_id == user_id)
            .Select(f => f.Who_id)
            .AsAsyncEnumerable();

        return (Task<IEnumerable<int>>)followers;
    }

    public bool IsFollowing(int who_id, int whom_id)
    {
        var follower = _context.Followers
            .Where(f => f.Who_id == who_id && f.Whom_id == whom_id)
            .FirstOrDefault();

        return follower != null;
    }
}