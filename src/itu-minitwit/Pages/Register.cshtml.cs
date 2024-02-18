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

    [ValidateAntiForgeryToken]
    public IActionResult OnPost()
    {
        if (Request.Cookies["username"] != null)
        {
            Console.WriteLine("Redirecting to public");
            return RedirectToPage("Public");
        }

        if (String.IsNullOrEmpty(Username))
        {
       		ModelState.AddModelError("", "You have to enter a username");
        } 
        else if (String.IsNullOrEmpty(Email))
        {
        	ModelState.AddModelError("", "You have to enter a valid email address");
        }
        else if (String.IsNullOrEmpty(Password))
        {
        	ModelState.AddModelError("", "You have to enter a password");
        }
        else if (Password != Password2)
    	{
        	ModelState.AddModelError("", "The two passwords do not match");
    	}
    	else if (DatabaseHandler.GetUserID(Username) != null)
    	{
       		ModelState.AddModelError("", "The username is already taken");
    	}
        
        if (!ModelState.IsValid)
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
