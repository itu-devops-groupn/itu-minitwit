using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; }
    public IEnumerable<string> Following { get; set; }
    public IEnumerable<string> Followers { get; set; }

    [BindProperty]
    [StringLength(160)]
    public string? Text { get; set; }
    [FromQuery(Name = "page")]
    public int PageIndex { get; set; } = 1;

    public PublicModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        Cheeps = new List<CheepDto>();
        Following = new List<string>();
        Followers = new List<string>();
    }

    public bool IsAuthenticated() {
        return User.Identity!.IsAuthenticated;
    }

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

    public async Task<IActionResult> OnGetAsync([FromQuery(Name = "page")] int pageIndex = 1)
    {
        if (IsAuthenticated()) {
            string userName = User.Identity!.Name!;
            
            if (!await _authorRepository.AuthorExists(userName))
            {
                var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
                await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
            }
        }
        var cheeps = await _cheepRepository.GetCheeps(pageIndex, 32);
        Cheeps = cheeps.ToList();
        Following =  _authorRepository.GetAuthorFollowing(User.Identity!.Name!);
        Followers = _authorRepository.GetAuthorFollowers(User.Identity!.Name!);
        return Page();
    }

    // post cheep
    public async Task<IActionResult> OnPostAsync()
    {
        if (!IsAuthenticated() || string.IsNullOrWhiteSpace(Text))
        {
            return RedirectToPage("Public");
        }

        string userName = User.Identity!.Name!;

        if (!await _authorRepository.AuthorExists(userName))
        {
            var email = User.Claims.Where(e => e.Type == "emails").Select(e => e.Value).SingleOrDefault();
            await _authorRepository.CreateAuthor(new CreateAuthorDto(userName, email!));
        }
        
        await _cheepRepository.CreateCheep(new CreateCheepDto(Text!, userName));

        return RedirectToPage("Public");
    }
    public async Task<IActionResult> OnPostFollow(string authorName){
        if (IsAuthenticated()) {
            await _authorRepository.FollowAuthor(User.Identity!.Name!, authorName);
        }
            return RedirectToPage("public");
    }

    public async Task<IActionResult> OnPostUnfollow(string authorName){
        if (IsAuthenticated()) {
            await _authorRepository.UnfollowAuthor(User.Identity!.Name!, authorName);
        }
            return RedirectToPage("public");
    }
}
