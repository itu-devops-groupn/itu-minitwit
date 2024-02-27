
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

    [HttpPost("/register")]
    public IActionResult Register(string username, string email, string password, string passwordRepeat)
    {
        string err = "";

        if(err.IsNullOrEmpty())
        {
            err = "You have to enter a username";
        }
        else if (email.IsNullOrEmpty() || !email.Contains('@'))
        {
            err = "You have to enter a valid email address";
        }
        else if (password != passwordRepeat)
        {
            err = "The two passwords do not match";
        }
        else if (_userRepository.GetUserId(username) != null)
        {
            err = "The username is already taken";
        }
        else
        {
            _userRepository.CreateUser(username, password, email);
        }

        if(err != "")
        {
            return BadRequest(err);
        }
        else
        {
            return NoContent();
        }

    }

}