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


        private void ConfigureElasticSearch(IServiceCollection services, ApplicationSettings appsettings)
        {

            var connectionSettings = new ElasticsearchClientSettings(new Uri(appsettings.ElasticSearch.Uri))
                .DefaultIndex(appsettings.ElasticSearch.ProductIndex)
                .DefaultMappingFor<ElasticImageVector>(m => m.IndexName(appsettings.ElasticSearch.ImageVectorIndex))
                .DefaultMappingFor<ElasticProduct>(m => m.IndexName(appsettings.ElasticSearch.ProductIndex))
                .DefaultMappingFor<ElasticCategory>(m => m.IndexName(appsettings.ElasticSearch.CategoryIndex))
                .DefaultMappingFor<ElasticManufacturer>(m => m.IndexName(appsettings.ElasticSearch.ManufacturerIndex))
                .DefaultMappingFor<ElasticProductTag>(m => m.IndexName(appsettings.ElasticSearch.ProductTagIndex))
                .DefaultMappingFor<ElasticSpecificationAttribute>(m => m.IndexName(appsettings.ElasticSearch.SpecificationAttributeIndex))
                .DefaultMappingFor<ElasticProductReview>(m => m.IndexName(appsettings.ElasticSearch.ProductReviewIndex))
                .DefaultMappingFor<ElasticProductExpectedRating>(m => m.IndexName(appsettings.ElasticSearch.ProductExpectedRatingIndex));




            services.AddSingleton(connectionSettings);

            services.AddTransient((sp) => 
            new ElasticsearchClient(sp.GetRequiredService<ElasticsearchClientSettings>()));

        }

     
        private async Task InitializeElasticSearch(IServiceProvider serviceProvider)
        {
            using (var scope= serviceProvider.CreateScope())
            {
                var appsettings = scope.ServiceProvider.GetRequiredService<ApplicationSettings>(); 

                var elasticClient = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();

                await elasticClient.Indices
                    .CreateAsync(ElasticIndeciesMapping.ImageVectorMappings(appsettings.ElasticSearch.ImageVectorIndex));

                await elasticClient.Indices
                    .CreateAsync(ElasticIndeciesMapping.ElasticProductMappings(appsettings.ElasticSearch.ProductIndex));

                await elasticClient.Indices
                    .CreateAsync(ElasticIndeciesMapping.ElasticCategoryMappings(appsettings.ElasticSearch.CategoryIndex));

                await elasticClient.Indices
                    .CreateAsync(ElasticIndeciesMapping.ElasticManufacturerMappings(appsettings.ElasticSearch.ManufacturerIndex));

                await elasticClient.Indices
                    .CreateAsync(ElasticIndeciesMapping.ElasticProductReviewMappings(appsettings.ElasticSearch.ProductReviewIndex));

                await elasticClient.Indices
                    .CreateAsync(ElasticIndeciesMapping.ElasticSpecificationAttributeMappings(appsettings.ElasticSearch.SpecificationAttributeIndex));

                await elasticClient.Indices
                    .CreateAsync(ElasticIndeciesMapping.ElasticProductTagMappings(appsettings.ElasticSearch.ProductTagIndex));

                await elasticClient.Indices
                    .CreateAsync(ElasticIndeciesMapping.ElasticProductExpectedRatingMappings(appsettings.ElasticSearch.ProductExpectedRatingIndex));

            }
        }

    }
}
