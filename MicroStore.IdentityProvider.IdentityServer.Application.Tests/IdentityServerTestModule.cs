using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework;
using Respawn;
using Respawn.Graph;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests
{
    [DependsOn(typeof(IdentityServerInfrastrcutreModule),
        typeof(IdentityServerApplicationModule),
        typeof(AbpAutofacModule),
        typeof(MediatorModule))]
    public class IdentityServerTestModule : AbpModule
    {

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationConfigurationDbContext>();

                dbContext.Database.Migrate();
            }
        }


        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var config = context.ServiceProvider.GetRequiredService<IConfiguration>();

                var respawner = Respawner.CreateAsync(config.GetConnectionString("DefaultConnection")!, new RespawnerOptions
                {
                    TablesToIgnore = new Table[]
                    {
                    "__EFMigrationsHistory"
                    }
                }).Result;

                respawner.ResetAsync(config.GetConnectionString("DefaultConnection")!).Wait();

            }
        }
    }
}
