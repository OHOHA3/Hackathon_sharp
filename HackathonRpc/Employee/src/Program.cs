using Employee.Rpc;
using Employee.Service;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureServices((_, services) =>
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<StartHackathonConsumer>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("rabbitmq","/", h =>
                {
                    h.Username("user");
                    h.Password("password");
                });
                var type = Environment.GetEnvironmentVariable("APP_TYPE") ?? throw new InvalidOperationException();
                var id = int.Parse(
                    Environment.GetEnvironmentVariable("APP_ID") ?? throw new InvalidOperationException());
                cfg.ReceiveEndpoint($"{type}_queue_{id}", e => { e.Consumer<StartHackathonConsumer>(ctx); });
            });
        });

        services.AddScoped<EmployeeService>();
    })
    .Build()
    .Run();