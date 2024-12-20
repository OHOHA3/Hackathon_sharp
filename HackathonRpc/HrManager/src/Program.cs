using HrManager.Manager;
using HrManager.Rpc;
using HrManager.Service;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Host.CreateDefaultBuilder(args).ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureServices((_, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<EmployeeInfoConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("rabbitmq","/", h =>
                {
                    h.Username("user");
                    h.Password("password");
                });
                cfg.ReceiveEndpoint("manager_queue", e =>
                {
                    e.Consumer<EmployeeInfoConsumer>(ctx);
                    e.UseConcurrencyLimit(1);
                });
            });
        });

        services.AddScoped<ITeamBuildingStrategy, TeamBuildStrategy>();
        services.AddScoped<HrManager.Manager.HrManager>();
        services.AddScoped<HrManagerService>();

        services.AddHttpClient();
    })
    .Build()
    .Run();