using HrManager.Manager;
using HrManager.Rest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:1228");

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddScoped<ITeamBuildingStrategy, TeamBuildStrategy>();
builder.Services.AddScoped<HrManager.Manager.HrManager>();
builder.Services.AddScoped<HrManagerService>();

var app = builder.Build();
app.MapControllers();
app.UseRouting();
app.Run();