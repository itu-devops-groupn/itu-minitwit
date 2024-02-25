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

                TempData["flash"] = "You were logged out";
            }

            return RedirectToPage("Public");
        }
    }
}
