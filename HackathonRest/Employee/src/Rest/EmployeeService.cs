using System.Text;
using System.Text.Json;
using Employee.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Employee.Rest;

public class EmployeeService(HttpClient httpClient, ILogger<EmployeeService> logger, IConfiguration configuration)
    : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var type = Environment.GetEnvironmentVariable("APP_TYPE") ?? throw new InvalidOperationException();
        var id = int.Parse(Environment.GetEnvironmentVariable("APP_ID") ?? throw new InvalidOperationException());

        logger.LogInformation($"Employee service starting, id: {id}, type: {type}");
        {
            var employeeInfo = GetEmployeeInfo(type, id);

            const string url = "http://hrmanager:1228/hr_manager/wishlist";
            var json = JsonSerializer.Serialize(employeeInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(url, content, stoppingToken).Result;
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Employee information sent successfully.");
            }
            else
            {
                logger.LogError($"Failed to send employee information. Status code: {response.StatusCode}");
            }
        }

        return Task.CompletedTask;
    }

    private EmployeeInfo GetEmployeeInfo(string type, int id)
    {
        //var type = configuration.GetValue<string>("type");
        // var id = configuration.GetValue<int>("id");

        var juniors =
            ScvEmployeeReader.Read("resources/Juniors5.csv");
        var teamLeads =
            ScvEmployeeReader.Read("resources/TeamLeads5.csv");

        return type is "teamLead"
            ? new EmployeeInfo(WishListCreator.CreateWishList(teamLeads, juniors)
                    .Find(list => list.EmployeeId == id)!,
                teamLeads.Find(e => e.Id == id)!,
                true)
            : new EmployeeInfo(WishListCreator.CreateWishList(juniors, teamLeads)
                    .Find(list => list.EmployeeId == id)!,
                juniors.Find(e => e.Id == id)!,
                false);
    }
}