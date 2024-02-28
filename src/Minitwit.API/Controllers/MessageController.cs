using Microsoft.AspNetCore.Mvc;

namespace Minitwit.API.Controllers;

public class MessageController : Controller
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessageController(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    private bool IsLoggedIn()
    {
        return HttpContext.Request.Headers["Authorization"] == "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh";
    }

    [HttpGet("/msgs")]
    public IActionResult GetMessages()
    {
        if(!IsLoggedIn())
        {
            return Forbid("You are not authorized to use this resource!");
        }

        var Messages = _messageRepository.GetMessages(30).Result;

        if (Messages == null)
        {
            return Ok();
        }

        return Ok(Messages.ToList());
    }

    [HttpGet("/msgs/{username}")]
    public async Task<IActionResult> GetMessages(string username)
    {
        if(!IsLoggedIn())
        {
            return Forbid("You are not authorized to use this resource!");
        }

        if(_userRepository.GetUserId(username).Result == 0)
        {
            return NotFound();
        }

        var Messages = _messageRepository.GetMessagesFromUser(username, 30).Result;

        if (Messages == null)
        {
            return Ok();
        }
        
        return Ok(Messages.ToList());
    }

    [HttpPost("/msgs/{username}")]
    public IActionResult AddMessage([FromBody] MessageRequestData data, string username)
    {
        var message = data.content;

        if (!IsLoggedIn())
        {
            return Forbid("You are not authorized to use this resource!");
        }

        if (_userRepository.GetUserId(username).Result == 0)
        {
            return NotFound();
        }

        _messageRepository.CreateMessage(message, _userRepository.GetUserId(username).Result);
        return Ok();
    }
}

public record MessageRequestData(string content);