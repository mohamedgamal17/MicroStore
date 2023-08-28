using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Operations;
using MicroStore.Catalog.Domain.Configuration;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.Infrastructure.ElasticSearch;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using MicroStore.Catalog.Infrastructure.Services;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
namespace MicroStore.Catalog.Infrastructure
{
    [DependsOn(typeof(CatalogApplicationOperationsModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpBlobStoringModule),
        typeof(AbpBlobStoringMinioModule))]
    public class CatalogInfrastructureModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var appSettings = configuration.Get<ApplicationSettings>();


            ConfigureMassTransit(context.Services, configuration);

            ConfigureElasticSearch(context.Services, appSettings);

            context.Services.AddAbpDbContext<CatalogDbContext>(opt =>
            {
                opt.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer(builder => builder.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName));

            });

            Configure<ImageDescriptorOptions>(opt =>
                    opt.Bins = new int[] { 8, 12, 3 }
                );
        }


        private void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = services.GetSingletonInstance<ApplicationSettings>();

            services.AddMassTransit(transitConfig =>
            {
                transitConfig.AddConsumers(typeof(CatalogApplicationOperationsModule).Assembly);

                transitConfig.UsingRabbitMq((ctx, rabbitConfig) =>
                {
                    rabbitConfig.Host(appSettings.MassTransit.Host, cfg =>
                    {
                        cfg.Username(appSettings.MassTransit.UserName);
                        cfg.Password(appSettings.MassTransit.Password);

                    });

                    rabbitConfig.ConfigureEndpoints(ctx);

                });

            });
        }
        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            await InitializeElasticSearch(context.ServiceProvider);
        }


        private void ConfigureElasticSearch(IServiceCollection services, ApplicationSettings applicationSettings)
        {
            var connectionSettings = new ElasticsearchClientSettings(new Uri(applicationSettings.ElasticSearch.Uri))
                .DefaultIndex(ElasticEntitiesConsts.ProductIndex)
                .DefaultMappingFor<ElasticImageVector>(m => m.IndexName(ElasticEntitiesConsts.ImageVectorIndex))
                .DefaultMappingFor<ElasticProduct>(m => m.IndexName(ElasticEntitiesConsts.ProductIndex))
                .DefaultMappingFor<ElasticCategory>(m => m.IndexName(ElasticEntitiesConsts.CategoryIndex))
                .DefaultMappingFor<ElasticManufacturer>(m => m.IndexName(ElasticEntitiesConsts.ManufacturerIndex))
                .DefaultMappingFor<ElasticProductTag>(m => m.IndexName(ElasticEntitiesConsts.ProductTagIndex))
                .DefaultMappingFor<ElasticSpecificationAttribute>(m => m.IndexName(ElasticEntitiesConsts.SpecificationAttributeIndex))
                .DefaultMappingFor<ElasticProductReview>(m => m.IndexName(ElasticEntitiesConsts.ProductReviewIndex))
                .DefaultMappingFor<ElasticProductExpectedRating>(m => m.IndexName(ElasticEntitiesConsts.ProductExpectedRatingIndex));




            services.AddSingleton(connectionSettings);

            services.AddTransient((sp) => 
            new ElasticsearchClient(sp.GetRequiredService<ElasticsearchClientSettings>()));

        }

     
        private async Task InitializeElasticSearch(IServiceProvider serviceProvider)
        {
            using (var scope= serviceProvider.CreateScope())
            {
                var elasticClient = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ImageVectorMappings());

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ElasticProductMappings());

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ElasticCategoryMappings());

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ElasticManufacturerMappings());

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ElasticProductReviewMappings());

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ElasticSpecificationAttributeMappings());

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ElasticProductTagMappings());

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ElasticProductExpectedRatingMappings());

            }
        }

    }
}
