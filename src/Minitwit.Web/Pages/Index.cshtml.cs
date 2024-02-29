using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Minitwit.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    public IEnumerable<MessageDto> Messages { get; set; }

    [BindProperty]
    public string? Text { get; set; }

    public IndexModel(IMessageRepository messageRepository,
                        IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        Messages = new List<MessageDto>();
    }
    public string? GetUserName() => Request.Cookies["username"];

    public string GetGravatar(string username)
    {
        return _userRepository.GetGravatarUrl(username, 48);
    }

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "no")] int no = 30)
    {
        if (GetUserName() == null)
        {
            return RedirectToPage("Public");
        }

        var messages = await _messageRepository.GetPersonalMessages(_userRepository.GetUserId(GetUserName()!).Result, no);
        Messages = messages.ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if(GetUserName() == null || string.IsNullOrWhiteSpace(Text))
        {
            Response.StatusCode = 401;
            return RedirectToPage();
        }

        await _messageRepository.CreateMessage(Text, _userRepository.GetUserId(GetUserName()!).Result);
        TempData["flash"] = "Your message was recorded";
        return RedirectToPage();
    }
}