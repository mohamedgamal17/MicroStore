using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Infrastructure.EntityFramework;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.EntityFrameworkCore;


namespace MicroStore.Shipping.Infrastructure
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEventBusModule))]
    public class ShippingInfrastructureModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new ShippingSystemProviderConventionalRegistar());
            context.Services.AddConventionalRegistrar(new UnitSystemProviderConventionalRegistar());


            ConfigureEntityFramework(context.Services);
        }


        private void ConfigureEntityFramework(IServiceCollection services)
        {
            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer();
            });

            services.AddAbpDbContext<ShippingDbContext>(cfg =>
            {
                cfg.AddDefaultRepositories(true);

                cfg.AddRepository<Shipment, ShipmentRepository>();
            });

        }

    }
}
