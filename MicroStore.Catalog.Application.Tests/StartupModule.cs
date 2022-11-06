

using MassTransit;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace MicroStore.Catalog.Application.Tests
{
    public class StartupModule : AbpModule
    {

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(Assembly.Load("MicroStore.Catalog.Application"));

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.ConfigureEndpoints(context);
                });

            });
        }


        public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            return base.OnApplicationInitializationAsync(context);
        }

        public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
        {
            return base.OnApplicationShutdownAsync(context);
        }
    }
}
