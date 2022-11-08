using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Infrastructure;
using MicroStore.Ordering.Infrastructure.EntityFramework;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.Ordering.Application.Tests
{
    [DependsOn(typeof(OrderApplicationModule),
        typeof(OrderInfrastructureModule),
        typeof(MediatorModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    public class StartupModule : AbpModule
    {
       
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {

                busRegisterConfig.AddConsumers(Assembly.Load("MicroStore.Ordering.Application"));

                busRegisterConfig.AddConsumers(Assembly.GetExecutingAssembly());

                busRegisterConfig.AddActivities(Assembly.Load("MicroStore.Ordering.Application"));

                busRegisterConfig.AddSagaStateMachine<OrderStateMachine, OrderStateEntity>()
                    .EntityFrameworkRepository(efConfig =>
                    {
                        efConfig.UseSqlServer();
                        efConfig.ExistingDbContext<OrderDbContext>();
                    });

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
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
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                if (dbContext.Database.EnsureCreated())
                {
                    dbContext.Database.Migrate();
                }
            }
        }



    }
}