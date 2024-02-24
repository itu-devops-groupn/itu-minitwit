namespace Chirp.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Chirp.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

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

    public bool IsFollowing(int who_id, int whom_id)
    {
        var follower = _context.Followers
            .Where(f => f.Who_id == who_id && f.Whom_id == whom_id)
            .FirstOrDefault();

        return follower != null;
    }
}