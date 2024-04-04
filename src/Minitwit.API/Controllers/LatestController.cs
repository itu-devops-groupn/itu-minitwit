using Microsoft.AspNetCore.Mvc;

namespace Minitwit.API.Controllers;
[Route("/")]
public class LatestController : Controller
{
    [HttpGet("latest")]
    public ActionResult GetLatest()
    {
        var latestInt = 0;
        try
        {
            latestInt = int.Parse(System.IO.File.ReadAllText("latest_processed_sim_action_id.txt"));
        }
        catch
        {
            latestInt = -1;
        }
        return Ok(new { latest = latestInt });
    }
}