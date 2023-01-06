using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Inventory.Application.Abstractions;
using MicroStore.Inventory.Infrastructure;
using MicroStore.Inventory.Infrastructure.EntityFramework;
using Respawn;
using Respawn.Graph;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.Inventory.Application.Tests
{

    [DependsOn(typeof(InventoryApplicationModule),
        typeof(InventoryApplicationAbstractionModule),
        typeof(InventoryInfrastructureModule),
        typeof(MediatorModule),
        typeof(AbpAutofacModule))]
    public class InventoryApplicationTestModule : AbpModule
    {
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(typeof(InventoryApplicationModule).Assembly);

                busRegisterConfig.UsingInMemory((context, inMemorybusConfig) =>
                {
                    inMemorybusConfig.ConfigureEndpoints(context);
                });
              
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {

            var loggerFactory = context.ServiceProvider.GetRequiredService<ILoggerFactory>();

            LogContext.ConfigureCurrentLogContext(loggerFactory);
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

                dbContext.Database.Migrate();

            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var config = context.ServiceProvider.GetRequiredService<IConfiguration>();

                var respawner = Respawner.CreateAsync(config.GetConnectionString("DefaultConnection")!, new RespawnerOptions
                {
                    TablesToIgnore = new Table[]
                    {
                    "__EFMigrationsHistory"
                    }
                }).Result;

                respawner.ResetAsync(config.GetConnectionString("DefaultConnection")!).Wait();

            }
        }
    }
}
