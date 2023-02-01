using Microsoft.EntityFrameworkCore.Design;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure;
using Volo.Abp.Autofac;
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


    [DependsOn(typeof(IdentityServerInfrastrcutreModule),
        typeof(IdentityServerApplicationModule),
        typeof(AbpAutofacModule),
        typeof(MediatorModule))]
    internal class IdentityServerDbContextFactoryModule : AbpModule
    {

    }


}
