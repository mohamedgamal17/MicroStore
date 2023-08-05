using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace MicroStore.IdentityProvider.Identity.Domain.Shared
{
    [DependsOn(typeof(AbpAutoMapperModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpFluentValidationModule))]
    public class IdentityDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<IdentityDomainSharedModule>();
            });

        }
    }
}