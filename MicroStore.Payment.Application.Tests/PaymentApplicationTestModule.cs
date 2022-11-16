using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Payment.Application.EntityFramework;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.Payment.Application.Tests
{
    [DependsOn(typeof(PaymentApplicationModule),
        typeof(MediatorModule),
        typeof(AbpAutofacModule))]
    public class PaymentApplicationTestModule : AbpModule
    {
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {

            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(typeof(PaymentApplicationModule).Assembly);

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
                var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

                if (dbContext.Database.EnsureCreated())
                {
                    dbContext.Database.Migrate();
                }
            }
        }

    }
}
