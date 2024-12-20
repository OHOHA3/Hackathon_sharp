using Hackathon.Db;
using Hackathon.Util;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hackathon.Hackathon;

public class HackathonRunner(
    HackathonEngine hackathonEngine,
    ILogger<HackathonRunner> logger,
    HackathonDbContext dbContext) : BackgroundService
{
    private const int SimCount = 1;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var juniors =
            ScvEmployeeReader.Read("resources/Juniors20.csv");
        var teamLeads =
            ScvEmployeeReader.Read("resources/TeamLeads20.csv");

        for (var i = 0; i < SimCount; i++)
        {
            var harmonic = hackathonEngine.Simulate(teamLeads, juniors);
            logger.LogInformation("harmonic: {harmonicValue}", harmonic);
        }

        var harmonicSum = dbContext.Hackathons.Sum(h => h.Harmonic);
        var hackathonCount = dbContext.Hackathons.Count();
        logger.LogInformation("mean harmonic: {harmonicSum}", harmonicSum / hackathonCount);

        return Task.CompletedTask;
    }
}