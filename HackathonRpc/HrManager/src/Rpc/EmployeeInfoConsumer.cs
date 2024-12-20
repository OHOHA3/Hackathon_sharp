using Contracts;
using HrManager.Service;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace HrManager.Rpc;

public class EmployeeInfoConsumer(ILogger<EmployeeInfoConsumer> logger, HrManagerService hrManagerService)
    : IConsumer<EmployeeInfo>
{
    public Task Consume(ConsumeContext<EmployeeInfo> context)
    {
        var employeeInfo = context.Message;

        logger.LogInformation($"Employee name: {employeeInfo.Employee.Name}");

        hrManagerService.HandleWishLists(employeeInfo);

        return Task.CompletedTask;
    }
}