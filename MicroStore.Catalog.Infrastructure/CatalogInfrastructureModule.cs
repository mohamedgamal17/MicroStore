using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using MicroStore.Catalog.Infrastructure.Services;
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

    }
}
