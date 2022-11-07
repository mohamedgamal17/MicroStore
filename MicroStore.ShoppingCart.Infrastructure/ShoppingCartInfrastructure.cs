using Microsoft.Extensions.DependencyInjection;
using MicroStore.ShoppingCart.Application.Abstraction;
using MicroStore.ShoppingCart.Infrastructure.EntityFramework;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MicroStore.ShoppingCart.Infrastructure
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule),
        typeof(ShoppingCartApplicationAbstractionModule))]
    public class ShoppingCartInfrastructureModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            context.Services.AddAbpDbContext<BasketDbContext>(cfg =>
            {
                cfg.AddDefaultRepositories(true);

            });

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer();
            });
        }
    }
}