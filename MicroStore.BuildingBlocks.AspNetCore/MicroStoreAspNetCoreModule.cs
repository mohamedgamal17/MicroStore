using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace MicroStore.BuildingBlocks.AspNetCore
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule),
        typeof(AbpSwashbuckleModule))]
    public class MicroStoreAspNetCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            //Configure<MvcOptions>(config =>
            //{
            //    config.Filters.Add(typeof(RequiredScopeAuthorizationHandler));
            //});

            context.Services.AddTransient<RequiredScopeAuthorizationHandler>();
        }
    }
}
