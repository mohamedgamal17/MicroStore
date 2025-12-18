using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Configuration;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace MicroStore.Shipping.Application.Abstraction
{
    [DependsOn(typeof(AbpAutoMapperModule),
        typeof(AbpValidationModule),
        typeof(AbpFluentValidationModule))]
    public class ShippingApplicationAbstractionModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            var appsettings = config.Get<ApplicationSettings>();

            context.Services.AddSingleton(appsettings);

            Configure<AbpAutoMapperOptions>(cfg =>
            {
                cfg.AddMaps<ShippingApplicationAbstractionModule>();
            });

        }
    }
}
