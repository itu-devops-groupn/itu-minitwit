
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Minitwit.API.Controllers;

public class AuthenticationController : Controller
{
    private readonly IUserRepository _userRepository;

    public AuthenticationController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private void UpdateLatest(int latest)
    {
        if(latest != -1)
        {
            System.IO.File.WriteAllText("latest_processed_sim_action_id.txt", latest.ToString());
        }
    }

    [HttpPost("/register")]
    public IActionResult Register([FromBody] RegisterRequestData data, [FromQuery(Name = "latest")] int latest = -1)
    {

        UpdateLatest(latest);

        var username = data.username;
        var email = data.email;
        var password = data.pwd;

        string err = "";

        if (username.IsNullOrEmpty())
        {
            err = "You have to enter a username";
        }
        else if (email.IsNullOrEmpty() || !email.Contains('@'))
        {
            err = "You have to enter a valid email address";
        }
        else if (password.IsNullOrEmpty())
        {
            err = "You have to enter a password";
        }
        else if (_userRepository.GetUserId(username).Result != 0)
        {
            err = "The username is already taken";
        }
        else
        {
            _userRepository.CreateUser(username, password, email);
        }

        if (err != "")
        {
            return BadRequest(err);
        }
        else
        {
            return Ok();
        }

    }

}

public record RegisterRequestData(int latest, string post_type, string username, string email, string pwd);