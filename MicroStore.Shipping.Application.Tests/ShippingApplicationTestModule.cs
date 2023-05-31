using MassTransit;
using MicroStore.Shipping.Infrastructure;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp;
using MicroStore.Shipping.Infrastructure.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.TestBase.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Respawn;
using Respawn.Graph;
using Microsoft.Extensions.Configuration;

namespace MicroStore.Shipping.Application.Tests
{
    [DependsOn(typeof(ShippingApplicationModule),
        typeof(ShippingInfrastructureModule),
        typeof(AbpAutofacModule))]
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
