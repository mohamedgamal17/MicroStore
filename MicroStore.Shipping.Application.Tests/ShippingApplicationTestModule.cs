using MassTransit;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Shipping.Infrastructure;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using MicroStore.Shipping.Application;
namespace MicroStore.Shipping.Application.Tests
{
    [DependsOn(typeof(ShippingApplicationModule),
        typeof(ShippingInfrastructureModule),
        typeof(AbpAutofacModule),
        typeof(MediatorModule))]
    public class ShippingApplicationTestModule : AbpModule
    {
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(typeof(ShippingApplicationModule).Assembly);

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.ConfigureEndpoints(context);
                });

            });
        }
    }
}
