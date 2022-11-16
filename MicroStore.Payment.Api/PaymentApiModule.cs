using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Payment.Application;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
namespace MicroStore.Payment.Api
{

    [DependsOn(typeof(PaymentApplicationModule),
        typeof(AbpAspNetCoreMvcModule),
         typeof(MediatorModule))]
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
