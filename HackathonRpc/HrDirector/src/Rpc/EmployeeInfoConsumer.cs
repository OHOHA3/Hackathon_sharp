using Contracts;
using HrDirector.Service;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace HrDirector.Rpc;

public class EmployeeInfoConsumer(ILogger<EmployeeInfoConsumer> logger, HrDirectorService hrDirectorService)
    : IConsumer<EmployeeInfo>
{
    public Task Consume(ConsumeContext<EmployeeInfo> context)
    {
        var employeeInfo = context.Message;

        logger.LogInformation($"Employee name: {employeeInfo.Employee.Name}");

        hrDirectorService.HandleWishLists(employeeInfo);

        return Task.CompletedTask;
    }
}