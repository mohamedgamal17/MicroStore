using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.Shipping.Application;
using MicroStore.Shipping.Infrastructure;
using Volo.Abp.Modularity;

namespace MicroStore.Shipping.WebApi
{

    [DependsOn(typeof(ShippingInfrastructureModule),
        typeof(ShippingApplicationModule),
        typeof(MicroStoreAspNetCoreModule))]
    public class ShippingApiModule : AbpModule
    {

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ShippingApiModule).Assembly);
            });
        }
    }

}
