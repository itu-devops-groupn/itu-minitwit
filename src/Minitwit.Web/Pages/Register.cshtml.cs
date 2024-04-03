using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Minitwit.Web.Pages;

[ValidateAntiForgeryToken]
public class RegisterModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private const string ErrorKey = "error";

    [BindProperty]
    public string? Username { get; set; }
    [BindProperty]
    public string? Password { get; set; }
    [BindProperty]
    public string? Email { get; set; }
    [BindProperty]
    public string? PasswordRepeat { get; set; }

    public RegisterModel(IUserRepository userRepository)
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

        if (Username == null)
        {
            ModelState.AddModelError(ErrorKey, "You have to enter a username");
            return Page();
        } 
        else if (Email == null)
        {
            ModelState.AddModelError(ErrorKey, "You have to enter a valid email address");
            return Page();
        }
        else if (Password == null)
        {
            ModelState.AddModelError(ErrorKey, "You have to enter a password");
            return Page();
        }
        else if (Password != PasswordRepeat)
        {
            ModelState.AddModelError(ErrorKey, "The two passwords do not match");
            return Page();
        }
        // Because this returns an int we check if it's 0 (no user) or not
        else if (_userRepository.GetUserId(Username).Result != 0)
        {
            ModelState.AddModelError(ErrorKey, "The username is already taken");
            return Page();
        }

        await _userRepository.CreateUser(Username, Password, Email);

        TempData["flash"] = "You were successfully registered and can login now";
        return RedirectToPage("Public");
    }
}