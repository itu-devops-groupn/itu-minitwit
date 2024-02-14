using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace itu_minitwit.Pages;

public class PublicModel : PageModel
{
    public List<Message> Messages = new();


    public IActionResult OnGet()
    {
        string query =
        $@"
        select message.*, user.* from message, user
        where message.flagged = 0 and message.author_id = user.user_id
        order by message.pub_date desc limit 30";
        var results = DatabaseHandler.QueryDB(query);

        Messages = results.Select(row => new Message
        {
            Text = row["text"],
            Username = row["username"],
            Email = row["email"],
            Date = DatabaseHandler.FormatDateTime(int.Parse(row["pub_date"])),
            GravatarUrl = DatabaseHandler.GravatarUrl(row["email"], 48)
        }).ToList();

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
