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
                    

                    var errorInfo = new ValidationProblemDetails(context.ModelState)
                    {
                        
                        Instance = context.HttpContext.Request.PathBase,
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    };

                    return new BadRequestObjectResult(errorInfo);
                };
            });

        }
    }
}
