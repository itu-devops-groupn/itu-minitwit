using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (Request.Cookies["username"] != null)
            {
                Response.Cookies.Delete("username");

                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddSeconds(2);
                Response.Cookies.Append("flash", "You were logged out", options);
            }

            return RedirectToPage("Public");
        }
    }
}
