using MicroStore.AspNetCore.UI;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.IdentityProvider.Identity.Domain.Shared;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MicroStore.IdentityProvider.IdentityServer.Web
{
    [DependsOn(typeof(IdentityServerInfrastrcutreModule),
        typeof(IdentityDomainSharedModule),
        typeof(MicroStoreAspNetCoreModule),
        typeof(MicroStoreAspNetCoreUIModule)
        )]
    public class IdentityServerWebModule :AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<IdentityServerWebModule>());

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<IdentityServerWebModule>("MicroStore.IdentityProvider.IdentityServer.Web");
            });
        }
    }
}