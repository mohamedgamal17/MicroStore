using MassTransit;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Catalog.Infrastructure;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.Catalog.Application.Tests
{
    [DependsOn(typeof(CatalogApplicationModule))]
    [DependsOn(typeof(CatalogInfrastructureModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(MediatorModule))]
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
