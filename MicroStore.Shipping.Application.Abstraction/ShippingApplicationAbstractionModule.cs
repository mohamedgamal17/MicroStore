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
            Configure<AbpAutoMapperOptions>(cfg =>
            {
                cfg.AddMaps<ShippingApplicationAbstractionModule>();
            });

        }
    }
}
