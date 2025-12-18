using MicroStore.AspNetCore.UI;
using MicroStore.BuildingBlocks.AspNetCore;
using MicroStore.IdentityProvider.Identity.Domain.Shared;
using MicroStore.IdentityProvider.OAuth.Infrastructure;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MicroStore.IdentityProvider.OAuth.Web
{
    [DependsOn(typeof(OAuthInfrastrcutreModule),
        typeof(IdentityDomainSharedModule),
        typeof(MicroStoreAspNetCoreModule),
        typeof(MicroStoreAspNetCoreUIModule)
        )]
    public class OAuthWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt => opt.AddMaps<OAuthWebModule>());

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<OAuthWebModule>("MicroStore.IdentityProvider.OAuth.Web");
            });
        }
    }
}