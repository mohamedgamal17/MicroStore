using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Emgu.CV.Ocl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application;
using MicroStore.Catalog.Domain.Configuration;
using MicroStore.Catalog.Domain.Entities;
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
    [DependsOn(typeof(CatalogApplicationModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpBlobStoringModule),
        typeof(AbpBlobStoringMinioModule))]
    public class CatalogInfrastructureModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            var appSettings = configuration.Get<ApplicationSettings>();


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


        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            await InitializeElasticSearch(context.ServiceProvider);
        }


        private void ConfigureElasticSearch(IServiceCollection services, ApplicationSettings applicationSettings)
        {
            var connectionSettings = new ElasticsearchClientSettings(new Uri(applicationSettings.ElasticSearch.Uri))
                .DefaultMappingFor<ImageVector>(m => m.IndexName(ImageVector.INDEX_NAME));


            services.AddSingleton(connectionSettings);

            services.AddTransient<ElasticsearchClient>();

        }
        private async Task InitializeElasticSearch(IServiceProvider serviceProvider)
        {
            using (var scope= serviceProvider.CreateScope())
            {
                var elasticClient = scope.ServiceProvider.GetRequiredService<ElasticsearchClient>();

                await elasticClient.Indices.CreateAsync(ElasticIndeciesMapping.ImageVectorMappings());
            }
        }

    }
}
