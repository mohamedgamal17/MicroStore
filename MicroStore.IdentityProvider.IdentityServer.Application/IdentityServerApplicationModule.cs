using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace MicroStore.IdentityProvider.IdentityServer.Application
{
    [DependsOn(typeof(AbpValidationModule),
        typeof(AbpAutoMapperModule))]
    public class IdentityServerApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<IdentityServerApplicationModule>();
            });
        }
    }
}