using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Infrastructure;
using MicroStore.Ordering.Infrastructure.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Respawn;
using Respawn.Graph;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.Ordering.Application.Tests
{
    [DependsOn(typeof(OrderApplicationModule),
        typeof(OrderInfrastructureModule))]
    [DependsOn(typeof(AbpAutofacModule))]
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

                busRegisterConfig.AddConsumers(typeof(OrderApplicationModule).Assembly);

                busRegisterConfig.AddConsumers(Assembly.GetExecutingAssembly());

                busRegisterConfig.AddActivities(typeof(OrderApplicationModule).Assembly);

                busRegisterConfig.AddSagaStateMachine<OrderStateMachine, OrderStateEntity>()
                    .EntityFrameworkRepository(efConfig =>
                    {
                        efConfig.UseSqlServer();
                        efConfig.ExistingDbContext<OrderDbContext>();
                    });

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
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                dbContext.Database.Migrate();
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var config = context.ServiceProvider.GetRequiredService<IConfiguration>();

            var respawner = Respawner.CreateAsync(config.GetConnectionString("DefaultConnection"), new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                    "__EFMigrationsHistory"
                }
            }).Result;


            respawner.ResetAsync(config.GetConnectionString("DefaultConnection")).Wait();
        }



    }
}