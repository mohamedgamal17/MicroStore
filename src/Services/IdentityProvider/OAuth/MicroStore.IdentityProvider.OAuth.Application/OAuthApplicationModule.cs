using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace MicroStore.IdentityProvider.OAuth.Application
{
    [DependsOn(typeof(AbpValidationModule),
        typeof(AbpAutoMapperModule))]
    public class OAuthApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<OAuthApplicationModule>();
            });
        }
    }
}