using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Prometheus;

namespace Minitwit.Web.Pages;

[ValidateAntiForgeryToken]
public class LoginModel : PageModel
{

    private static readonly Histogram LoadLoginDuration = Metrics.CreateHistogram
    (
        "web_login_load_duration_seconds",
        "Time to load messages personal page in seconds"
    );

    [BindProperty]
    public string? Username { get; set; }
    [BindProperty]
    public string? Password { get; set; }
    private readonly IUserRepository _userRepository;

    public LoginModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IActionResult OnGet()
    {
        if (Request.Cookies["username"] != null)
        {
            return RedirectToPage("Public");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Request.Cookies["username"] != null)
        {
            return RedirectToPage("Public");
        }

        if(string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ModelState.AddModelError("error", "You have to enter a username and a password");
            return Page();
        }

        var result = await _userRepository.GetUserForLogin(Username);

        if (result == null)
        {
            ModelState.AddModelError("error", "Invalid username");
            return Page();
        }
        
        if (result.Password != Password)
        {
            ModelState.AddModelError("error", "Invalid password");
            return Page();
        }

        using(LoadLoginDuration.NewTimer())
        {
            // Cookies for username
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(10);
            Response.Cookies.Append("username", Username, options);

            // Flashmessage
            TempData["flash"] = "You were logged in";
            return RedirectToPage("Public");
        }
    }
}
