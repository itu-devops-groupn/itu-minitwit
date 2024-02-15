using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace itu_minitwit.Pages;

[BindProperties]
public class LoginModel : PageModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (Request.Cookies["username"] != null)
        {
            Console.WriteLine("Redirecting to public");
            return RedirectToPage("Public");
        }

        string query = 
        @"SELECT * FROM user 
        WHERE Username = @username";
        object[] args = { new SqliteParameter("@username", Username) };
        List<Dictionary<string, string>> result = DatabaseHandler.QueryDB(query, args);
        if (result.Count == 0)
        {
            Console.WriteLine("No user found");
            return Page();
        }
        
        if (result[0]["pw_hash"] != Password)
        {
            Console.WriteLine("Wrong Password");
            return Page();
        }
        
        Console.WriteLine("Succesful login");
        CookieOptions options = new CookieOptions();
        options.Expires = DateTime.Now.AddMinutes(10);
        Response.Cookies.Append("username", Username, options);
        Console.WriteLine("Username: " + Username);
        return Page();
    }
}

