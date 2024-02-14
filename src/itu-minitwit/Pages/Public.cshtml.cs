using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace itu_minitwit.Pages;

public class PublicModel : PageModel
{
    public List<Message> Messages = new();

    public IActionResult OnGet()
    {
        
        return Page();
    }

}

public record Message
{
    public string Text { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Date { get; set; }
    public string GravatarUrl { get; set; }
}
