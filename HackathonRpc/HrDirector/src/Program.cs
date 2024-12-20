using HrDirector.Db;
using HrDirector.Rest;
using HrDirector.Rpc;
using HrDirector.Service;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:1337");

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddLogging();

builder.Services.AddScoped<HrDirector.Director.HrDirector>();
builder.Services.AddScoped<HrDirectorService>();

builder.Services.AddDbContext<HackathonDbContext>(options => options
    .UseNpgsql("Host=host.docker.internal;Database=postgres;Username=postgres;Password=12345")
    .EnableSensitiveDataLogging());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmployeeInfoConsumer>();
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq","/", h =>
        {
            h.Username("user");
            h.Password("password");
        });
        cfg.ReceiveEndpoint("director_queue", e =>
        {
            e.Consumer<EmployeeInfoConsumer>(ctx);
            e.UseConcurrencyLimit(1);
        });
    });
});


var app = builder.Build();

app.MapControllers();
app.UseRouting();

using (var scope = app.Services.CreateScope())
{
    var hrDirectorService = scope.ServiceProvider.GetRequiredService<HrDirectorService>();
    hrDirectorService.StartHackathon();
}

app.Run();