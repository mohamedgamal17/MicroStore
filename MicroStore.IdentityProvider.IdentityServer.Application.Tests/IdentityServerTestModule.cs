using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure;
using MicroStore.IdentityProvider.IdentityServer.Infrastructure.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Respawn;
using Respawn.Graph;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
namespace MicroStore.IdentityProvider.IdentityServer.Application.Tests
{
    [DependsOn(typeof(IdentityServerInfrastrcutreModule),
        typeof(IdentityServerApplicationModule),
        typeof(AbpAutofacModule))]
    public class IdentityServerTestModule : AbpModule
    {
        private readonly JsonSerializerSettings _jsonSerilizerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DomainModelContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        };

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            context.Services.AddIdentityServer().AddConfigurationStore<ApplicationConfigurationDbContext>(cfg =>
            {
                cfg.DefaultSchema = IdentityServerDbConsts.ConfigurationSchema;

                cfg.ConfigureDbContext = (builder) =>
                {
                    builder.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlServerOpt =>
                    {
                        sqlServerOpt.MigrationsAssembly(typeof(IdentityServerInfrastrcutreModule).Assembly.FullName)
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                    });


                };
            }).AddOperationalStore<ApplicationPersistedGrantDbContext>(cfg =>
                {

                    cfg.DefaultSchema = IdentityServerDbConsts.OperationalSchema;

                    cfg.ConfigureDbContext = (builder) =>
                    {
                        builder.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlServerOpt =>
                        {
                            sqlServerOpt.MigrationsAssembly(typeof(IdentityServerInfrastrcutreModule).Assembly.FullName)
                                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                        });
                    };
                });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
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
