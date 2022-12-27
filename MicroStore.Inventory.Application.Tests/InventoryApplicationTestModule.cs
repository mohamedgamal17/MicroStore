using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Inventory.Application.Abstractions;
using MicroStore.Inventory.Infrastructure;
using MicroStore.Inventory.Infrastructure.EntityFramework;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.Inventory.Application.Tests
{

    [DependsOn(typeof(InventoryApplicationModule),
        typeof(InventoryAbstractionModule),
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

    }
}
