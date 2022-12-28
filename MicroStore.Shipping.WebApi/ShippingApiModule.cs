using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Shipping.Application;
using MicroStore.Shipping.Infrastructure;
using Volo.Abp.Modularity;

namespace MicroStore.Shipping.WebApi
{

    [DependsOn(typeof(ShippingInfrastructureModule),
        typeof(ShippingApplicationModule),
        typeof(MediatorModule))]
    public class ShippingApiModule : AbpModule
    {

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
          
        }
    }

}
