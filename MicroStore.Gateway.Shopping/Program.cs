using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using MicroStore.Gateway.Shopping;
using MicroStore.Gateway.Shopping.Exceptions;
using MicroStore.Gateway.Shopping.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using Serilog.Events;
Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        .AddOcelot($"etc/{hostingContext.HostingEnvironment.EnvironmentName.ToLower()}",hostingContext.HostingEnvironment)
        .AddEnvironmentVariables();

   
});


builder.Services.ConfigureCoreServices(builder.Configuration);

builder.Services.AddMvc();

builder.Services.AddProblemDetails(config =>
{
    config.MapToStatusCode<InvalidTokenException>(StatusCodes.Status400BadRequest);

    config.Map<InvalidTokenException>((ex) =>
    {
        return new ProblemDetails
        {
            Type = ex.ErrorType,
            Title = ex.Error,
            Detail = ex.ErrorDesceription,
            Status = StatusCodes.Status400BadRequest,
        };
    });

 
});

builder.Host.UseSerilog();

var app = builder.Build();

app.UseProblemDetails();

app.UseAuthentication();


await app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();

