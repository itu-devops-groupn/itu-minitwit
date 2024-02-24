using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    public IEnumerable<MessageDto> Messages { get; set; }

    [BindProperty]
    public string? Text { get; set; }
    
    public PublicModel(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        Messages = new List<MessageDto>();
    }
    public string? GetUserName() => Request.Cookies["username"];

    public string GetGravatar(string username) {
        return _userRepository.GetGravatarUrl(username, 48);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var messages = await _messageRepository.GetMessages(30);
        Messages = messages.ToList();
        return Page();
    }

    // // post cheep
    // public async Task<IActionResult> OnPostAsync()
    // {
    //     if (!IsAuthenticated() || string.IsNullOrWhiteSpace(Text))
    //     {
    //         return RedirectToPage("Public");
    //     }

    //     string userName = User.Identity!.Name!;

    //     if (!await _userRepository.AuthorExists(userName))
    //     {
    //         var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
    //         await _userRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
    //     }
        
    //     await _messageRepository.CreateCheep(new CreateCheepDto(Text!, userName));

    //     return RedirectToPage("Public");
    // }
    // public async Task<IActionResult> OnPostFollow(string authorName){
    //     if (IsAuthenticated()) {
    //         await _userRepository.FollowAuthor(User.Identity!.Name!, authorName);
    //     }
    //         return RedirectToPage("public");
    // }

    // public async Task<IActionResult> OnPostUnfollow(string authorName){
    //     if (IsAuthenticated()) {
    //         await _userRepository.UnfollowAuthor(User.Identity!.Name!, authorName);
    //     }
    //         return RedirectToPage("public");
    // }
}
