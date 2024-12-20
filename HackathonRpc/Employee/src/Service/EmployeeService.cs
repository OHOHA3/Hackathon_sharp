using Contracts;
using Employee.Util;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Employee.Service;

public class EmployeeService(ILogger<EmployeeService> logger, IBusControl busControl)
{
    public void Handle()
    {
        var type = Environment.GetEnvironmentVariable("APP_TYPE") ?? throw new InvalidOperationException();
        var id = int.Parse(Environment.GetEnvironmentVariable("APP_ID") ?? throw new InvalidOperationException());

        logger.LogInformation($"Employee service starting, id: {id}, type: {type}");

        var employeeInfo = GetEmployeeInfo(type, id);
        busControl.Publish(employeeInfo);

        logger.LogInformation("Employee information sent to RabbitMQ successfully.");
    }

    private EmployeeInfo GetEmployeeInfo(string type, int id)
    {
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