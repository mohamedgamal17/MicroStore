using MassTransit;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Shipping.Infrastructure;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using MicroStore.Shipping.Application;
using Volo.Abp;
using MicroStore.Shipping.Infrastructure.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Respawn.Graph;
using Microsoft.Extensions.Configuration;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.TestBase.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MicroStore.Shipping.Application.Tests
{
    [DependsOn(typeof(ShippingApplicationModule),
        typeof(ShippingInfrastructureModule),
        typeof(AbpAutofacModule),
        typeof(MediatorModule))]
    public class ShippingApplicationTestModule : AbpModule
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
                busRegisterConfig.AddConsumers(typeof(ShippingApplicationModule).Assembly);

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
                var dbContext = scope.ServiceProvider.GetRequiredService<ShippingDbContext>();

                dbContext.Database.Migrate();

                SeedShipmentData(dbContext);

                SeedShipmentSystemsData(dbContext);
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ShippingDbContext>();

                dbContext.Database.EnsureDeleted();

            }
        }
        private void SeedShipmentData(ShippingDbContext dbContext)
        {
            using (var stream = new StreamReader(@"Dummies\Shipments.json"))
            {
                var json = stream.ReadToEnd();

                var data = JsonConvert.DeserializeObject<JsonWrapper<Shipment>>(json, _jsonSerilizerSettings);

                if (data != null)
                {
                    dbContext.Shipments.AddRange(data.Data);
                }

                dbContext.SaveChanges();
            }
        }

        private void SeedShipmentSystemsData(ShippingDbContext dbContext)
        {
            using (var stream = new StreamReader(@"Dummies\ShipmentSystems.json"))
            {
                var json = stream.ReadToEnd();

                var data = JsonConvert.DeserializeObject<JsonWrapper<ShippingSystem>>(json, _jsonSerilizerSettings);

                if (data != null)
                {
                    dbContext.ShippingSystems.AddRange(data.Data);
                }

                dbContext.SaveChanges();
            }
        }
    }
}
