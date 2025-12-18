using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Autofac;
using MicroStore.IdentityProvider.OAuth.Infrastructure.EntityFramework;
using MicroStore.IdentityProvider.OAuth.Application;
using MicroStore.IdentityProvider.OAuth.Infrastructure;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests.EntityFramework
{
    public class ApplicationConfigurationDbContextFactory : IDesignTimeDbContextFactory<ApplicationConfigurationDbContext>
    {
        public ApplicationConfigurationDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();

            var application = services.AddApplication<IdentityServerDbContextFactoryModule>();

            var serviceProvider = services.BuildServiceProviderFromFactory();

            application.Initialize(serviceProvider);

            return application.ServiceProvider.GetRequiredService<ApplicationConfigurationDbContext>();
        }
        
    }


    public class ApplicationPersistedGrantDbContextFactory : IDesignTimeDbContextFactory<ApplicationPersistedGrantDbContext>
    {
        public ApplicationPersistedGrantDbContext CreateDbContext(string[] args)
        {
            var services = new ServiceCollection();

            var application = services.AddApplication<IdentityServerDbContextFactoryModule>();

            var serviceProvider = services.BuildServiceProviderFromFactory();

            application.Initialize(serviceProvider);

            return application.ServiceProvider.GetRequiredService<ApplicationPersistedGrantDbContext>();
        }

    }


    [DependsOn(typeof(OAuthInfrastrcutreModule),
        typeof(OAuthApplicationModule),
        typeof(AbpAutofacModule))]
    internal class IdentityServerDbContextFactoryModule : AbpModule
    {

    }


}
