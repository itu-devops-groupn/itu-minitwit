using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

[ValidateAntiForgeryToken]
public class RegisterModel : PageModel
{

    private readonly IUserRepository _userRepository;

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
       		ModelState.AddModelError("error", "You have to enter a username");
            return Page();
        } 
        else if (Email == null)
        {
        	ModelState.AddModelError("error", "You have to enter a valid email address");
            return Page();
        }
        else if (Password == null)
        {
        	ModelState.AddModelError("error", "You have to enter a password");
            return Page();
        }
        else if (Password != PasswordRepeat)
    	{
        	ModelState.AddModelError("error", "The two passwords do not match");
            return Page();   	
        }   
        // Because this returns an int we check if its 0 (no user) or not
    	else if (_userRepository.GetUserId(Username).Result != 0)
    	{
       		ModelState.AddModelError("error", "The username is already taken");
            return Page();
    	}

        await _userRepository.CreateUser(Username, Password, Email);
        return RedirectToPage("Public");
    }

}