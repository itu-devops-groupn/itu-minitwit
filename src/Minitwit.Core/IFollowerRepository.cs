namespace Minitwit.Core;

public interface IFollowerRepository
{
    Task CreateFollower(int who_id, int whom_id);
    Task DeleteFollower(int who_id, int whom_id);
    Task<bool> IsFollowing(int who_id, int whom_id);
    Task<IEnumerable<int>> GetFollowers(int user_id);

}