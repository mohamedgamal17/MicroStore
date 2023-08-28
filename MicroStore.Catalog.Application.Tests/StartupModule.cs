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
using MicroStore.Catalog.Infrastructure.ElasticSearch;
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

            ConfigureElasticSearch(context.Services, appsettings);
        }

        private void ConfigureElasticSearch(IServiceCollection services , ApplicationSettings applicationSettings)
        {
            var connectionSettings = new ElasticsearchClientSettings(new Uri(applicationSettings.ElasticSearch.Uri))
                .DefaultIndex(ElasticTestIndices.ProductIndex)
                .DefaultMappingFor<ElasticImageVector>(m => m.IndexName(ElasticTestIndices.ImageVectorIndex))
                .DefaultMappingFor<ElasticProduct>(m => m.IndexName(ElasticTestIndices.ProductIndex))
                .DefaultMappingFor<ElasticCategory>(m => m.IndexName(ElasticTestIndices.CategoryIndex))
                .DefaultMappingFor<ElasticManufacturer>(m => m.IndexName(ElasticTestIndices.ManufacturerIndex))
                .DefaultMappingFor<ElasticProductTag>(m => m.IndexName(ElasticTestIndices.ProductTagIndex))
                .DefaultMappingFor<ElasticSpecificationAttribute>(m => m.IndexName(ElasticTestIndices.SpecificationAttributeIndex))
                .DefaultMappingFor<ElasticProductReview>(m => m.IndexName(ElasticTestIndices.ProductReviewIndex))
                .DefaultMappingFor<ElasticProductExpectedRating>(m => m.IndexName(ElasticTestIndices.ProductExpectedRatingIndex));

            services.AddSingleton(connectionSettings);


            services.AddTransient((sp) => new ElasticsearchClient(sp.GetRequiredService<ElasticsearchClientSettings>()));
           
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


        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var elasticClient = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();

                 elasticClient.Indices.Create(ElasticIndeciesMapping.ImageVectorMappings());

                 elasticClient.Indices.Create(ElasticIndeciesMapping.ElasticProductMappings());

                 elasticClient.Indices.Create(ElasticIndeciesMapping.ElasticCategoryMappings());

                 elasticClient.Indices.Create(ElasticIndeciesMapping.ElasticManufacturerMappings());

                 elasticClient.Indices.Create(ElasticIndeciesMapping.ElasticProductTagMappings());

                 elasticClient.Indices.Create(ElasticIndeciesMapping.ElasticProductReviewMappings());

                 elasticClient.Indices.Create(ElasticIndeciesMapping.ElasticSpecificationAttributeMappings());

                 elasticClient.Indices.Create(ElasticIndeciesMapping.ElasticProductExpectedRatingMappings());

            }
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

            elasticClient.Indices.Delete(IndexName.From<ElasticProductTag>());

            elasticClient.Indices.Delete(IndexName.From<ElasticSpecificationAttribute>());

            elasticClient.Indices.Delete(IndexName.From<ElasticProductExpectedRating>());
        }

    }
}
