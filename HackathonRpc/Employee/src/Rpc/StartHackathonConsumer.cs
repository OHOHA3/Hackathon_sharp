using Contracts;
using Employee.Service;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Employee.Rpc;

public class StartHackathonConsumer(ILogger<StartHackathonConsumer> logger, EmployeeService employeeService)
    : IConsumer<StartMessage>
{
    public Task Consume(ConsumeContext<StartMessage> context)
    {
        logger.LogInformation("Starting HackathonConsumer");

        employeeService.Handle();

        return Task.CompletedTask;
    }
}