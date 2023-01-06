using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
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


builder.Services.AddOcelot();

builder.Services.AddSwaggerForOcelot(builder.Configuration, swagg =>
{
    swagg.GenerateDocsForGatewayItSelf = true;


});

builder.Services.AddMvc();

builder.Host.UseSerilog();

var app = builder.Build();


app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";

    opt.ReConfigureUpstreamSwaggerJson = (_, swaggerJson) =>
    {
        var swagger = JObject.Parse(swaggerJson);
        // ... alter upstream json
        return swagger.ToString(Formatting.Indented);
    };
});
app.UseOcelot().Wait();

app.MapGet("/", () => "Hello World!");

app.Run();
