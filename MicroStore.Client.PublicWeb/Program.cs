//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Hosting;
//using MicroStore.Client.PublicWeb;
//using MicroStore.Client.PublicWeb.Consts;

//var builder = WebApplication.CreateBuilder(args);


//var app = builder.ConfigureService();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();

//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication();

//app.UseAuthorization();

//app.MapRazorPages();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapDefaultControllerRoute();

//    endpoints.MapControllers();

//    endpoints.MapAreaControllerRoute(name: AreaNames.Administration,
//        areaName: AreaNames.Administration,
//        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//});


//app.Run();


using Serilog.Events;
using Serilog;
using MicroStore.Client.PublicWeb;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("Logs/logs.txt"))
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

try
{
    Log.Information("Starting web host.");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.AddAppSettingsSecretsJson()
        .UseAutofac()
        .UseSerilog();
    await builder.AddApplicationAsync<PublicWebModule>();

    var app = builder.Build();
    await app.InitializeApplicationAsync();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush(); ;
}
