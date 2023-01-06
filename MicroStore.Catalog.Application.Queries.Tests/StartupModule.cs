using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Infrastructure;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Respawn;
using Respawn.Graph;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
namespace MicroStore.Catalog.Application.Queries.Tests
{
   
    

    [DependsOn(typeof(CatalogApplicationModule))]
    [DependsOn(typeof(CatalogInfrastructureModule))]
    [DependsOn(typeof(AbpAutofacModule))]
    [DependsOn(typeof(MediatorModule))]
    public class StartupModule : AbpModule
    {

        private readonly JsonSerializerSettings _jsonSerilizerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DomainModelContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        };

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(typeof(CatalogApplicationModule).Assembly);

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.ConfigureEndpoints(context);
                });

            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                dbContext.Database.Migrate();

                SeedCategoriesData(dbContext);

                SeedProductsData(dbContext);
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

        private void SeedCategoriesData(CatalogDbContext dbContext)
        {
            using (var stream = new StreamReader(@"Dummies\Categories.json"))
            {
                var json = stream.ReadToEnd();
                var dummy = JsonConvert.DeserializeObject<Dummy<Category>>(json, _jsonSerilizerSettings);

                if (dummy != null)
                {
                    dbContext.Categories.AddRange(dummy.Data);
                }

                dbContext.SaveChanges();
            }
        }


        private void SeedProductsData(CatalogDbContext dbContext)
        {
            using (var stream = new StreamReader(@"Dummies\Products.json"))
            {
                var json = stream.ReadToEnd();

                var dummy = JsonConvert.DeserializeObject<Dummy<Product>>(json, _jsonSerilizerSettings);

                if (dummy != null)
                {
                    dbContext.Products.AddRange(dummy.Data);
                }

                dbContext.SaveChanges();

            }

        }
    }
}