using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
namespace MicroStore.IdentityProvider.Identity.Application
{
    [DependsOn(typeof(AbpDddApplicationModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpAutoMapperModule))]
    public class IdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<IdentityApplicationModule>();
            });

        }
    }
}