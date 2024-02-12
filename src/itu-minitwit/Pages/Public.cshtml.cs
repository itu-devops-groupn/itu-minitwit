using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace itu_minitwit.Pages
{
    public class PublicModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
