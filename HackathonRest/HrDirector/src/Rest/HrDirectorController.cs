using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HrDirector.Rest;

[Route("hr_director")]
[ApiController]
public class HrDirectorController(ILogger<HrDirectorController> logger, HrDirectorService service) : ControllerBase
{
    [Route("teams")]
    [HttpPost]
    public IActionResult ReceiveTeams([FromBody] TeamsInfo? teamsInfo)
    {
        if (teamsInfo == null)
        {
            logger.LogWarning("Received null employee information.");
            return BadRequest("Employee information cannot be null.");
        }

        logger.LogInformation($"Received {teamsInfo.Teams.Count} teams");

        var harmonic = service.CalculateHarmonic(teamsInfo);
        logger.LogInformation($"Harmonic of teams: {harmonic}");

        service.SaveHackathon(teamsInfo, harmonic);

        var meanHarmonic = service.GetMeanHarmonic();
        logger.LogInformation("mean harmonic: {harmonicSum}", meanHarmonic);

        return Ok(new { status = "Teams information received successfully", teamsCount = teamsInfo.Teams.Count });
    }
}