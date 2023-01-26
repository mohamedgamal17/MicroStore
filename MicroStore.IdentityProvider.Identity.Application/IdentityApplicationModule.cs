using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
namespace MicroStore.IdentityProvider.Identity.Application
{
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