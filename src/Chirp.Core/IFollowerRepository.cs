namespace Chirp.Core;

public interface IFollowerRepository
{
    Task CreateFollower(int who_id, int whom_id);
    Task DeleteFollower(int who_id, int whom_id);
}