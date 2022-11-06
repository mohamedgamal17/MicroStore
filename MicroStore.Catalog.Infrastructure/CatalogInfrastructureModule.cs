using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions;
using MicroStore.Catalog.Infrastructure.EntityFramework;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MicroStore.Catalog.Infrastructure
{
    [DependsOn(typeof(CatalogApplicationAbstractionModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class CatalogInfrastructureModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CatalogDbContext>(opt =>
            {
                opt.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer(builder => builder.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName));

            });
        }
    }
}
