using MicroStore.BuildingBlocks.Mediator;
using MicroStore.TestBase.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using MassTransit;
using MicroStore.Payment.Application.EntityFramework;
using Volo.Abp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MicroStore.Payment.Domain;
using Respawn;
using Respawn.Graph;
using Microsoft.Extensions.Configuration;

namespace MicroStore.Payment.Application.Queries.Tests
{
    [DependsOn(typeof(PaymentApplicationModule),
        typeof(MediatorModule),
        typeof(AbpAutofacModule))]
    public class StartupModule : AbpModule
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
                busRegisterConfig.AddConsumers(typeof(PaymentApplicationModule).Assembly);

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
                var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

                dbContext.Database.Migrate();

                SeedPaymentsData(dbContext);

                SeedPaymentSystemsData(dbContext);

            }
        }


        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
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
        private void SeedPaymentsData(PaymentDbContext dbContext)
        {
            using (var stream = new StreamReader(@"Dummies\Payments.json"))
            {
                var json = stream.ReadToEnd();

                var data = JsonConvert.DeserializeObject<JsonWrapper<PaymentRequest>>(json,_jsonSerilizerSettings);

                if(data!= null)
                {
                    dbContext.PaymentRequests.AddRange(data.Data);
                }

                dbContext.SaveChanges();
            }
        }

        private void SeedPaymentSystemsData(PaymentDbContext dbContext)
        {
            using(var stream= new StreamReader(@"Dummies\PaymentSystems.json"))
            {
                var json = stream.ReadToEnd();

                var data = JsonConvert.DeserializeObject<JsonWrapper<PaymentSystem>>(json, _jsonSerilizerSettings);

                if(data != null)
                {
                    dbContext.PaymentSystems.AddRange(data.Data);
                }

                dbContext.SaveChanges();
            }
        }
    }
}
