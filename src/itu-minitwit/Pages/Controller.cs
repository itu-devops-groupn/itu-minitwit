using itu_minitwit;
using Microsoft.AspNetCore.Mvc;
using System;

namespace itu_minitwit.Pages;

[ApiController]
public class MessageController : ControllerBase
{
	public static int? GetUserIdFromSession()
    	{
    		var username = "rokk"; // OBS! find name of logged in user
    		return DatabaseHandler.GetUserID(username);
    	}
    	
	[HttpPost("add_message")]
	public IActionResult AddMessage([FromBody] string text)
	{
	    try
	    {
		// Check if the user is authenticated
		//if (!User.Identity.IsAuthenticated)
		//{
		//    return StatusCode(401, new { error = "Unauthorized" });
		//}

		using var connection = DatabaseHandler.ConnectDB();
		using var command = connection.CreateCommand();
		command.CommandText =
		$@"
		INSERT INTO message (author_id, text, pub_date, flagged) 
		VALUES (@authorId, @text, @pubDate, 0)";
		command.Parameters.AddWithValue("@authorId", GetUserIdFromSession());
		command.Parameters.AddWithValue("@text", text);
		command.Parameters.AddWithValue("@pubDate", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
		command.ExecuteNonQuery();

		return Ok(new { message = "Message added successfully" });
	    }
	    catch (Exception ex)
	    {
		return StatusCode(500, new { error = "An error occurred while adding the message" });
	    }
	}
	
	[HttpPost("{username}/unfollow")]
    	public IActionResult UnfollowUser(string username)
    	{  
     	   //if (!User.Identity.IsAuthenticated)
     	   //{
     	   //    return StatusCode(401); // Unauthorized
     	   //}
	
    	   var whomId = DatabaseHandler.GetUserID(username);
    	   Console.WriteLine(whomId);
     	   if (whomId == null)
     	   {
     	       return NotFound(); // 404 Not Found
     	   }

     	   using var connection = DatabaseHandler.ConnectDB();
     	   using var command = connection.CreateCommand();
	   command.CommandText =
	   $@"
	   DELETE FROM follower WHERE who_id = @userId AND whom_id = @whomId";
     	   command.Parameters.AddWithValue("@userId", GetUserIdFromSession());
     	   command.Parameters.AddWithValue("@whomId", whomId);
     	   command.ExecuteNonQuery();

     	   //TempData["Message"] = $"You are no longer following \"{username}\"";
     	   //return RedirectToAction("UserTimeline", new { username = username });
     	   return RedirectToPage("/Public");
    	}
    	
    	[HttpPost("{username}/follow")]
    	public IActionResult FollowUser(string username)
    	{  
     	   //if (!User.Identity.IsAuthenticated)
     	   //{
     	   //    return StatusCode(401); // Unauthorized
     	   //}
	
    	   var whomId = DatabaseHandler.GetUserID(username);
    	   Console.WriteLine(whomId);
     	   if (whomId == null)
     	   {
     	       return NotFound(); // 404 Not Found
     	   }

     	   using var connection = DatabaseHandler.ConnectDB();
     	   using var command = connection.CreateCommand();
	   command.CommandText =
	   $@"
	   INSERT INTO follower (who_id, whom_id) VALUES (@userId, @whomId)";
     	   command.Parameters.AddWithValue("@userId", GetUserIdFromSession());
     	   command.Parameters.AddWithValue("@whomId", whomId);
     	   command.ExecuteNonQuery();

     	   //TempData["Message"] = $"You are now following \"{username}\"";
     	   //return RedirectToAction("UserTimeline", new { username = username });
     	   return RedirectToPage("/Public");
    	}
}
