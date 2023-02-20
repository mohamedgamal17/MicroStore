using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.AspNetCore.Security;
using MicroStore.BuildingBlocks.Results.Http;
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
            Configure<MvcOptions>(config =>
            {
                config.Filters.Add(typeof(RequiredScopeAuthorizationHandler));

            });


          
            context.Services.AddTransient<RequiredScopeAuthorizationHandler>();
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var result = context.ModelState.ConvertModelStateErrors();

                    var errorInfo = ErrorInfo.Validation("One or more validation error occured.", result);


                    return new ObjectResult(errorInfo);
                };
            });

        }
    }
}
