using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;
public class AboutMeModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public IEnumerable<Claim> FilteredClaims { get; private set; }
    public IEnumerable<CheepDto> Cheeps { get; set; }
    public IEnumerable<string> Following { get; set; }
    public IEnumerable<string> Followers { get; set; }

    public AboutMeModel(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
    {
        _authorRepository = authorRepository;
        _cheepRepository = cheepRepository;
        FilteredClaims = new List<Claim>();
        Cheeps = new List<CheepDto>();
        Following = new List<string>();
        Followers = new List<string>();
    }

    public bool IsAuthenticated()
    {
        return User.Identity!.IsAuthenticated;
    }

    public bool IsCurrentAuthor(string authorName)
    {
        return User.Identity!.IsAuthenticated && authorName == User.Identity!.Name;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!IsAuthenticated())
        {
            return RedirectToPage("Public");
        }

        string userName = User.Identity!.Name!;

        if (!await _authorRepository.AuthorExists(userName))
        {
            var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
            await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
        }
        
        var cheeps = await _cheepRepository.GetCheepsFromAuthor(userName, 1, int.MaxValue);
        Cheeps = cheeps.ToList();
        var desiredClaimTypes = new List<string> { "name", "emails" };
        FilteredClaims = User.Claims.Where(c => desiredClaimTypes.Contains(c.Type));
        Following = _authorRepository.GetAuthorFollowing(userName).ToList();
        Followers = _authorRepository.GetAuthorFollowers(userName).ToList();
        return Page();
    }

    public async Task<IActionResult> OnPostForgetAuthorAsync()
    {
        await _authorRepository.DeleteAuthor(User.Identity!.Name!);
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("Public");
    }

    public async Task<IActionResult> OnPostDeleteCheepAsync(Guid cheepId)
    {
        await _cheepRepository.DeleteCheep(cheepId);
        return RedirectToPage("AboutMe");
    }

    public async Task<IActionResult> OnPostFollowAsync(string authorName)
    {
        if (IsAuthenticated())
        {
            await _authorRepository.FollowAuthor(User.Identity!.Name!, authorName);
        }
        return RedirectToPage("AboutMe");
    }

    public async Task<IActionResult> OnPostUnfollowAsync(string authorName)
    {
        if (IsAuthenticated())
        {
            await _authorRepository.UnfollowAuthor(User.Identity!.Name!, authorName);
        }
        return RedirectToPage("AboutMe");
    }
}
