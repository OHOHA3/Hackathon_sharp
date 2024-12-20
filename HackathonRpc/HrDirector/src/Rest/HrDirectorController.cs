using Contracts;
using HrDirector.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrDirector.Rest;

[Route("hr_director")]
[ApiController]
public class HrDirectorController(ILogger<HrDirectorController> logger, HrDirectorService hrDirectorService)
    : ControllerBase
{
    [Route("teams")]
    [HttpPost]
    public IActionResult ReceiveTeams([FromBody] TeamsInfo? teamsInfo)
    {
        if (teamsInfo == null)
        {
            logger.LogWarning("Received null teams.");
            return BadRequest("Teams cannot be null.");
        }

        logger.LogInformation($"Received {teamsInfo.Teams.Count} teams");

        hrDirectorService.HandleTeams(teamsInfo);

        return Ok(new { status = "Teams received successfully", teamsCount = teamsInfo.Teams.Count });
    }
}