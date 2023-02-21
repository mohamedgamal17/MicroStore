using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.BuildingBlocks.Security;
using MicroStore.Payment.Application;
using Volo.Abp.Modularity;
namespace MicroStore.Payment.Api
{

    [DependsOn(typeof(PaymentApplicationModule),
        typeof(MicroStoreAspNetCoreModule),
        typeof(MicroStoreSecurityModule))]
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
