using Microsoft.AspNetCore.Mvc;

namespace Minitwit.API.Controllers;

public class MessageController : Controller
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    bool isLoggedIn;

    public MessageController(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;

        isLoggedIn = HttpContext.Request.Headers["Authorization"] == "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh";
    }

    [HttpGet("/msgs")]
    public IActionResult GetMessages()
    {
        if(!isLoggedIn)
        {
            return Forbid("You are not authorized to use this resource!");
        }

        var Messages = _messageRepository.GetMessages(30).Result.ToList();
        return Ok(Messages);
    }

    [HttpGet("/msgs/{username}")]
    public IActionResult GetMessages(string username)
    {
        if(!isLoggedIn)
        {
            return Forbid("You are not authorized to use this resource!");
        }

        if(_userRepository.GetUserId(username).Result == 0)
        {
            return NotFound();
        }

        var Messages = _messageRepository.GetMessagesFromUser(username, 30).Result.ToList();
        return Ok(Messages);
    }

    [HttpPost("/msgs/{username}")]
    public IActionResult AddMessage(string username)
    {
        if(!isLoggedIn)
        {
            return Forbid("You are not authorized to use this resource!");
        }

        if(_userRepository.GetUserId(username).Result == 0)
        {
            return NotFound();
        }

        using var reader = new StreamReader(HttpContext.Request.Body);
        var jsonRead = reader.ReadToEnd();
        var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(jsonRead);
        var message = dict["content"];

        _messageRepository.CreateMessage(message, _userRepository.GetUserId(username).Result);
        return NoContent();
    }



}