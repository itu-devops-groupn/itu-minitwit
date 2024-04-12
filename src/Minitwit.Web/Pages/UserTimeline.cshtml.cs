using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Minitwit.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFollowerRepository _followerRepository;
    public IEnumerable<MessageDto> Messages { get; set; }
    public bool IsFollowing { get; set; }

    [BindProperty]
    [StringLength(160)]
    public string? Text { get; set; }
    [FromQuery(Name = "page")]
    public int PageIndex { get; set; } = 1;

    public UserTimelineModel(
        IMessageRepository messageRepository,
        IUserRepository userRepository,
        IFollowerRepository followerRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _followerRepository = followerRepository;
        Messages = new List<MessageDto>();
    }

    public string? GetUserName() => Request.Cookies["username"];

    public string GetGravatar(string username)
    {
        return _userRepository.GetGravatarUrl(username, 48);
    }

    public async Task<IActionResult> OnGetAsync(string authorName)
    {
        var messages = await _messageRepository.GetMessagesFromUser(authorName, 30);
        Messages = messages.ToList();

        if (GetUserName() != null)
        {
            IsFollowing = await _followerRepository.IsFollowing(
                _userRepository.GetUserId(GetUserName()!).Result,
                _userRepository.GetUserId(authorName).Result);
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (GetUserName() == null || string.IsNullOrWhiteSpace(Text))
        {
            Response.StatusCode = 401;
            return RedirectToPage();
        }

        await _messageRepository.CreateMessage(Text, _userRepository.GetUserId(GetUserName()!).Result);
        TempData["flash"] = "Your message was recorded";
        return RedirectToPage();
    }

    /*

        FOLLOW AND UNFOLLOW
    
    */

    public async Task<IActionResult> OnPostFollow(string authorName)
    {
        if (GetUserName == null)
        {
            return StatusCode(401);
        }

        var whom_id = await _userRepository.GetUserId(authorName);

        if (whom_id == -1)
        {
            return StatusCode(404);
        }

        var who_id = await _userRepository.GetUserId(GetUserName()!);

        await _followerRepository.CreateFollower(who_id, whom_id);

        TempData["flash"] = "You are now following " + authorName;

        return RedirectToPage("UserTimeline");
    }

    public async Task<IActionResult> OnPostUnfollow(string authorName)
    {
        if (GetUserName == null)
        {
            return StatusCode(401);
        }

        var whom_id = await _userRepository.GetUserId(authorName);

        if (whom_id == -1)
        {
            return StatusCode(404);
        }

        var who_id = await _userRepository.GetUserId(GetUserName()!);

        await _followerRepository.DeleteFollower(who_id, whom_id);

        TempData["flash"] = "You are no longer following " + authorName;
        return RedirectToPage("UserTimeline");
    }


}

/* PLEASE DELETE SOON
    public bool IsCurrentAuthor(string authorName) {
        return User.Identity!.IsAuthenticated && authorName == User.Identity!.Name;
    }

    public int NextPage() {
        if (Cheeps.Count() < 32) {
            return PageIndex;
        }
        return PageIndex + 1;
    }

    public int PreviousPage() {
        if (PageIndex == 1) {
            return 1;
        }
        return PageIndex - 1;
    }
    public async Task<IActionResult> OnGetAsync(string authorName, [FromQuery(Name = "page")] int pageIndex = 1)
    {
        Following =  _us.GetAuthorFollowing(User.Identity!.Name!);
        Followers = _us.GetAuthorFollowers(User.Identity!.Name!);
        if (IsCurrentAuthor(authorName))
        {
            Cheeps = (await _.GetPersonalCheeps(authorName, pageIndex, 32)).ToList();
            FollowingCount = Following.Count();
            FollowersCount = Followers.Count();
        }
        else
        {
            Cheeps = (await _.GetCheepsFromAuthor(authorName, pageIndex, 32)).ToList();
            FollowingCount = _us.GetAuthorFollowing(authorName).Count();
            FollowersCount = _us.GetAuthorFollowers(authorName).Count();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!IsAuthenticated() || string.IsNullOrWhiteSpace(Text))
        {
            return RedirectToPage("UserTimeline");
        }

        string userName = User.Identity!.Name!;

        if (!await _us.AuthorExists(userName))
        {
            var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
            await _us.CreateAuthor(new CreateAuthorDto(userName, email!));
        }

        await _.CreateCheep(new CreateCheepDto(Text, userName));

        return RedirectToPage("UserTimeline");
    }

    public async Task<IActionResult> OnPostFollowAsync(string authorName){
        if (IsAuthenticated()) {
            await _us.FollowAuthor(User.Identity!.Name!, authorName);
        }
        return RedirectToPage("UserTimeline");
    }

    public async Task<IActionResult> OnPostUnfollowAsync(string authorName){
        if (IsAuthenticated()) {
            await _us.UnfollowAuthor(User.Identity!.Name!, authorName);
        }   
        return RedirectToPage("UserTimeline");
    }
}
*/
