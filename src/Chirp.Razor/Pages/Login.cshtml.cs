using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

[ValidateAntiForgeryToken]
public class LoginModel : PageModel
{

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

        // Cookies for username
        CookieOptions options = new CookieOptions();
        options.Expires = DateTime.Now.AddMinutes(10);
        Response.Cookies.Append("username", Username, options);

        // Cookie for flash
        options.Expires = DateTime.Now.AddSeconds(2);
        Response.Cookies.Append("flash", "You were logged in", options);
        return RedirectToPage("Public");
    }
}
