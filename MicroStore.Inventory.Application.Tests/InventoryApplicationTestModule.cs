using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MicroStore.Inventory.Infrastructure;
using MicroStore.Inventory.Infrastructure.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Respawn;
using Respawn.Graph;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.Domain.OrderAggregate;

namespace MicroStore.Inventory.Application.Tests
{

    [DependsOn(typeof(InventoryApplicationModule),
        typeof(InventoryInfrastructureModule),
        typeof(AbpAutofacModule))]
    public class InventoryApplicationTestModule : AbpModule
    {
        private readonly JsonSerializerSettings _jsonSerilizerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DomainModelContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },

            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(typeof(InventoryApplicationModule).Assembly);

                busRegisterConfig.UsingInMemory((context, inMemorybusConfig) =>
                {
                    inMemorybusConfig.ConfigureEndpoints(context);
                });
              
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {

            var loggerFactory = context.ServiceProvider.GetRequiredService<ILoggerFactory>();

            LogContext.ConfigureCurrentLogContext(loggerFactory);
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

                dbContext.Database.Migrate();
                SeedProductsData(dbContext);

                SeedOrderData(dbContext);
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

        private void SeedOrderData(InventoryDbContext dbContext)
        {
            using (var stream = new StreamReader(@"Dummies/Orders.json"))
            {
                var json = stream.ReadToEnd();

                var jsonWrapper = JsonConvert.DeserializeObject<JsonWrapper<Order>>(json, _jsonSerilizerSettings);

                if (jsonWrapper != null)
                {
                    dbContext.Orders.AddRange(jsonWrapper.Data);
                }

                dbContext.SaveChanges();
            }
        }

        private void SeedProductsData(InventoryDbContext dbContext)
        {
            using (var stream = new StreamReader(@"Dummies/Products.json"))
            {
                var json = stream.ReadToEnd();

                var jsonWrapper = JsonConvert.DeserializeObject<JsonWrapper<Product>>(json, _jsonSerilizerSettings);

                if (jsonWrapper != null)
                {
                    dbContext.Products.AddRange(jsonWrapper.Data);
                }

                dbContext.SaveChanges();
            }
        }
    }
}
