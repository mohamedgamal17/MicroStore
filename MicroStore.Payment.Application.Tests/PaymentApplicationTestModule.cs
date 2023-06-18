using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Application.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Respawn;
using Respawn.Graph;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Application.Domain;

namespace MicroStore.Payment.Application.Tests
{
    [DependsOn(typeof(PaymentApplicationModule),
        typeof(AbpAutofacModule))]
    public class PaymentApplicationTestModule : AbpModule
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


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

                dbContext.Database.Migrate();

                SeedFakePaymentMethod(dbContext);

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


        private  void SeedFakePaymentMethod(PaymentDbContext dbContext)
        {
            var paymentMethods = new List<PaymentSystem>
            {
                new PaymentSystem
                {
                    Name = PaymentMethodConst.PaymentGatewayName,
                    IsEnabled = true,
                    DisplayName = PaymentMethodConst.PaymentGatewayName,
                    Image = PaymentMethodConst.PaymentGatewayName,

                },

                new PaymentSystem
                {
                   Name = PaymentMethodConst.NonActiveGateway,
                    IsEnabled = false,
                    DisplayName = PaymentMethodConst.NonActiveGateway,
                    Image = PaymentMethodConst.NonActiveGateway,
                }
            };

            dbContext.AddRange(paymentMethods);

            dbContext.SaveChanges();
        }

    }
}
