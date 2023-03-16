using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.Payment.Application;
using Volo.Abp.Modularity;
namespace MicroStore.Payment.Api
{

    [DependsOn(typeof(PaymentApplicationModule),
        typeof(MicroStoreAspNetCoreModule))]
    public class PaymentApiModule : AbpModule
    {

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(PaymentApiModule).Assembly);
            });
        }


    }

}
