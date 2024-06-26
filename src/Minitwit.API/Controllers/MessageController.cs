using Microsoft.AspNetCore.Mvc;

namespace Minitwit.API.Controllers;
[Route("/")]
public class MessageController : Controller
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public MessageController(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    private void UpdateLatest(int latest)
    {
        if (latest != -1)
        {
            System.IO.File.WriteAllText("latest_processed_sim_action_id.txt", latest.ToString());
        }
    }

    private bool IsLoggedIn()
    {
        /* retrieve the token from the Authorization header. Use it for development
        and replace token with second part of authHeader to get through via swagger. */
        var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
        return authHeader == "Basic c2ltdWxhdG9yOnN1cGVyX3NhZmUh" || authHeader == "Basic cmFzbXVzOmZvb2Jhcg==";
    }

    [HttpGet("msgs")]
    public async Task<IActionResult> GetMessages([FromQuery(Name = "latest")] int latest = -1, [FromQuery(Name = "no")] int no = 30)
    {
        UpdateLatest(latest);

        if (!IsLoggedIn())
        {
            return StatusCode(403, new { status = 403, error_msg = "You are not authorized to use this resource!" });
        }

        var Messages = await _messageRepository.GetMessages(no);

        if (Messages == null)
        {
            return Ok();
        }

        return Ok(Messages.Select(m => new { content = m.Text, user = m.Username, pub_date = m.Pub_date }));
    }

    [HttpGet("msgs/{username}")]
    public async Task<IActionResult> GetMessages(string username, [FromQuery(Name = "latest")] int latest = -1, [FromQuery(Name = "no")] int no = 30)
    {
        UpdateLatest(latest);

        if (!IsLoggedIn())
        {
            return StatusCode(403, new { status = 403, error_msg = "You are not authorized to use this resource!" });
        }

        if (_userRepository.GetUserId(username).Result == 0)
        {
            return NotFound();
        }

        var Messages = await _messageRepository.GetMessagesFromUser(username, no);

        if (Messages == null)
        {
            return Ok();
        }

        return Ok(Messages.Select(m => new { content = m.Text, user = m.Username, pub_date = m.Pub_date }));
    }

    [HttpPost("msgs/{username}")]
    public async Task<IActionResult> AddMessage([FromBody] MessageRequestData data, string username, [FromQuery(Name = "latest")] int latest = -1)
    {
        UpdateLatest(latest);

        var message = data.content;

        if (!IsLoggedIn())
        {
            return StatusCode(403, new { status = 403, error_msg = "You are not authorized to use this resource!" });
        }

        if (_userRepository.GetUserId(username).Result == 0)
        {
            return NotFound();
        }

        await _messageRepository.CreateMessage(message, _userRepository.GetUserId(username).Result);
        return NoContent();
    }
}

public record MessageRequestData(string content);