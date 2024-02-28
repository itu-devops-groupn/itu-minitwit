using Microsoft.AspNetCore.Mvc;

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

    private bool IsLoggedIn()
    {
        return HttpContext.Request.Headers["Authorization"] == "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh";
    }

    [HttpGet("/fllws/{username}")]
    public IActionResult GetFollowers(string username)
    {
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
    public IActionResult ModifyFollow(string username)
    {
        if (!IsLoggedIn())
        {
            return Forbid("You are not authorized to use this resource!");
        }

        // read content of request
        var body = new StreamReader(Request.Body).ReadToEndAsync().Result;
        var elements = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(body);
        var command = elements["post_type"];

        if(command.Equals("unfollow"))
        {
            _followerRepository.DeleteFollower(
                _userRepository.GetUserId(username).Result,
                _userRepository.GetUserId(elements["unfollow"]).Result
            );

            return NoContent();
        }

        _followerRepository.CreateFollower(
            _userRepository.GetUserId(username).Result,
            _userRepository.GetUserId(elements["follow"]).Result
        );

        return NoContent();

    }


}