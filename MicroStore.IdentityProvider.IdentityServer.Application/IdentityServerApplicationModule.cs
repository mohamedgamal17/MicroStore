using MicroStore.BuildingBlocks.InMemoryBus;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MicroStore.IdentityProvider.IdentityServer.Application
{
    [DependsOn(typeof(InMemoryBusModule))]
    public class IdentityServerApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<IdentityServerApplicationModule>(true);
            });
        }
    }
}