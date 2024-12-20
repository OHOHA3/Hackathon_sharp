using Hackathon.Db;
using Hackathon.Hackathon;
using Hackathon.Hr;
using Hackathon.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hackathon;

public static class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((_, services) =>
            {
                services.AddDbContext<HackathonDbContext>(options => options
                    .UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=12345")
                    .EnableSensitiveDataLogging());
                services.AddScoped<ITeamBuildingStrategy, TeamBuildStrategy>();
                services.AddScoped<HrManager>();
                services.AddScoped<HrDirector>();
                services.AddScoped<HackathonEngine>();
                services.AddScoped<IHostedService, HackathonRunner>();
            })
            .Build();

        host.Run();
    }
}