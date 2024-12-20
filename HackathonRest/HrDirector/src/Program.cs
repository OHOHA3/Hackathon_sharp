using HrDirector.Db;
using HrDirector.Rest;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:1337");

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddScoped<HrDirector.Director.HrDirector>();
builder.Services.AddScoped<HrDirectorService>();
builder.Services.AddDbContext<HackathonDbContext>(options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException())
    .EnableSensitiveDataLogging());

var app = builder.Build();
app.MapControllers();
app.UseRouting();
app.Run();