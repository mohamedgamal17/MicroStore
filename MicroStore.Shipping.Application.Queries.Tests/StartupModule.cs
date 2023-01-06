using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Mediator;
using MicroStore.Shipping.Infrastructure;
using MicroStore.Shipping.Infrastructure.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using MicroStore.Shipping.Domain.Entities;

namespace MicroStore.Shipping.Application.Queries.Tests
{
    [DependsOn(typeof(ShippingApplicationModule),
        typeof(ShippingInfrastructureModule),
        typeof(AbpAutofacModule),
        typeof(MediatorModule))]
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

                if(data != null)
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
