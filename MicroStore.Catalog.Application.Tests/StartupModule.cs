using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Infrastructure;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using MicroStore.TestBase.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Respawn;
using Respawn.Graph;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using MicroStore.Catalog.Application.Operations;
using MicroStore.Catalog.Domain.Configuration;
using Elastic.Clients.Elasticsearch;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Tests
{
    [DependsOn(typeof(CatalogInfrastructureModule))]
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
            var configuration = context.Services.GetConfiguration();

            var appsettings = configuration.Get<ApplicationSettings>();

            ConfigureMassTransit(context.Services);
        }


        private void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(typeof(CatalogApplicationOperationsModule).Assembly);

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

                RemoveElasticSearchIndcies(scope.ServiceProvider);
            }
        }

        private void RemoveElasticSearchIndcies(IServiceProvider serviceProvider)
        {
            var elasticClient = serviceProvider.GetRequiredService<ElasticsearchClient>();

            elasticClient.Indices.Delete(IndexName.From<ElasticImageVector>());

            elasticClient.Indices.Delete(IndexName.From<ElasticProduct>());

            elasticClient.Indices.Delete(IndexName.From<ElasticCategory>());

            elasticClient.Indices.Delete(IndexName.From<ElasticManufacturer>());

            elasticClient.Indices.Delete(IndexName.From<ElasticTag>());

            elasticClient.Indices.Delete(IndexName.From<ElasticSpecificationAttribute>());

            elasticClient.Indices.Delete(IndexName.From<ElasticProductExpectedRating>());
        }

    }
}
