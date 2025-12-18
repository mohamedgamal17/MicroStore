using Microsoft.Extensions.DependencyInjection;
using MicroStore.Geographic.Application.EntityFramework;
using Volo.Abp.Modularity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Respawn;
using Volo.Abp;
using Microsoft.Extensions.Configuration;
using Respawn.Graph;

namespace MicroStore.Geographic.Application.Tests
{
    [DependsOn(typeof(GeographicApplicationModule),
        typeof(AbpAutofacModule))]
    public class GeographicApplicationTestModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GeographicDbContext>();

                dbContext.Database.Migrate();
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
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
