

using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.ShoppingCart.Infrastructure;
using MicroStore.ShoppingCart.Infrastructure.EntityFramework;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.ShoppingCart.Application.Tests
{
    [DependsOn(typeof(ShoppingCartApplicationModule),
        typeof(ShoppingCartInfrastructureModule),
        typeof(MediatorModule),
        typeof(AbpAutofacModule))]
    public class ShoppingCartApplicationTestModule : AbpModule
    {
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(typeof(ShoppingCartApplicationModule).Assembly);

                busRegisterConfig.AddConsumers(Assembly.GetExecutingAssembly());

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.UseConsumeFilter(typeof(ConsumerUnitOfWorkFilter<>), context);

                    inMemoryBusConfig.ConfigureEndpoints(context);

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
                var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();

                if (dbContext.Database.EnsureCreated())
                {
                    dbContext.Database.Migrate();
                }
            }

        }

         

    }
}
