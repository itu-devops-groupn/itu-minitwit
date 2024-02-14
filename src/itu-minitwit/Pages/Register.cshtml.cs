using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
