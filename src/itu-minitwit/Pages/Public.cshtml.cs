using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{
    public class PublicModel : PageModel
    {
        public void OnGet()
        {
            return Page();
        }
    }
}
