using PetFamily.API;
using PetFamily.API.Extensions;
using PetFamily.API.Middlewares;
using PetFamily.Application;
using PetFamily.Infrastructure;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException("Seq"))
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSerilog();

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddVolunteersApplication()
    .AddApi();

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    await app.ApplyMigrations();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Use(async (context, next) =>
{
    Console.WriteLine(context.Request.Path +  " start");
    await next.Invoke();
}); 

app.Run();

namespace PetFamily.API
{
    public partial class Program
    {
        // This partial class is used to allow the Program class to be extended in tests.
        // It is necessary for the WebApplicationFactory to work correctly in integration tests.
    }
}