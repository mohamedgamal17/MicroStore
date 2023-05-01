using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.IdentityServer.Application;
using MicroStore.IdentityProvider.IdentityServer.Application.Common;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework;
using Volo.Abp.Modularity;

namespace MicroStore.IdentityProvider.IdentityServer.Infrastructure
{
    [DependsOn(typeof(IdentityServerApplicationModule))]
    public class IdentityServerInfrastrcutreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            
        }
    }
}