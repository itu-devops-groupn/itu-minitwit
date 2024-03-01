using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Minitwit.API.Controllers;

public class FollowerController : Controller
{
    private readonly IFollowerRepository _followerRepository;
    private readonly IUserRepository _userRepository;

    public FollowerController(IFollowerRepository followerRepository, IUserRepository userRepository)
    {
        _followerRepository = followerRepository;
        _userRepository = userRepository;
    }

    private void UpdateLatest(int latest)
    {
        if(latest != -1)
        {
            System.IO.File.WriteAllText("latest_processed_sim_action_id.txt", latest.ToString());
        }
    }

    private bool IsLoggedIn()
    {
        return HttpContext.Request.Headers["Authorization"] == "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh";
    }

    [HttpGet("/fllws/{username}")]
    public IActionResult GetFollowers(string username, [FromQuery(Name = "latest")] int latest = -1)
    {
        UpdateLatest(latest);
        if(!IsLoggedIn())
        {
            return Forbid("You are not authorized to use this resource!");
        }

        if(_userRepository.GetUserId(username).Result == 0)
        {
            return NotFound();
        }

        var followersIds = _followerRepository.GetFollowers(_userRepository.GetUserId(username).Result).Result;
        followersIds = followersIds.Take(100).ToList();
        var result = new Dictionary<string, List<string>>
        {
            ["follows"] = []
        };
        foreach (var id in followersIds)
        {
            var followersUsername = _userRepository.GetUsername(id).Result;
            result["follows"].Add(followersUsername);
        }

        return Ok(result);
    }

    [HttpPost("/fllws/{username}")]
    public IActionResult ModifyFollow(string username, [FromBody] FollowRequestData data, [FromQuery(Name = "latest")] int latest = -1)
    {
        UpdateLatest(latest);

        if (!IsLoggedIn())
        {
            return Forbid("You are not authorized to use this resource!");
        }

        if(!data.unfollow.IsNullOrEmpty())
        {
            _followerRepository.DeleteFollower(
                _userRepository.GetUserId(username).Result,
                _userRepository.GetUserId(data.unfollow).Result
            );
        }
        else {
            _followerRepository.CreateFollower(
                _userRepository.GetUserId(username).Result,
                _userRepository.GetUserId(data.follow).Result
            );
        }


        return Ok();

    }
}

public record FollowRequestData(string follow, string unfollow);