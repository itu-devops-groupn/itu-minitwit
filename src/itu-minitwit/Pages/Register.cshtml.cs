using itu_minitwit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace itu_minitwit.Pages;

[BindProperties]
public class RegisterModel : PageModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Password2 { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (HttpContext.Session.GetString("username") != null)
        {
            return RedirectToPage("/");
        }

        if (String.IsNullOrEmpty(Username)
        || String.IsNullOrEmpty(Email)
        || String.IsNullOrEmpty(Password)
        || DatabaseHandler.GetUserID(Username) != null
        || Password != Password2)
        {
            return Page();
        }


        using var connection = DatabaseHandler.ConnectDB();
        using var command = connection.CreateCommand();
        command.CommandText =
        $@"
            INSERT INTO user (username, email, pw_hash) 
            values ($username, $email, $password)";
        command.Parameters.AddWithValue("$username", Username);
        command.Parameters.AddWithValue("$email", Email);
        command.Parameters.AddWithValue("$password", Password);

        using var reader = command.ExecuteReader();
        connection.Close();

        return RedirectToPage("Public");
    }

}
