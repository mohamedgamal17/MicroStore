using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
namespace MicroStore.Catalog.Infrastructure
{
    [DependsOn(typeof(CatalogApplicationAbstractionModule),
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

            ConfigureMinio(configuration);
        }


        private void ConfigureMinio(IConfiguration configuration)
        {
            //Configure<AbpBlobStoringOptions>(options =>
            //{
            //    options.Containers.ConfigureDefault(container =>
            //    {
            //        container.UseMinio(minio =>
            //        {
            //            minio.EndPoint = configuration.GetValue<string>("Minio:EndPoint");
            //            minio.AccessKey = configuration.GetValue<string>("Minio:AccessKey");
            //            minio.SecretKey = configuration.GetValue<string>("Minio:SecretKey");
            //            minio.BucketName = configuration.GetValue<string>("Minio:Bucket");
            //            minio.CreateBucketIfNotExists = true;
            //        });
            //    });
            //});
            
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseMinio(minio =>
                    {
                        minio.EndPoint = "localhost:9000";
                        minio.AccessKey = "lcTx1milctOS1BoJ";
                        minio.SecretKey = "78q8T617AAU4aUzALCMzQIECLWdmedKw";
                        minio.BucketName = "test-catalog-product-image";
                        minio.CreateBucketIfNotExists = true;
                        minio.WithSSL = false;
                       
                    });
                });
            });
        }
    }
}
