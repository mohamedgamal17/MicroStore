using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.PluginInMemoryTest.EntityFramework;
using MicroStore.Shipping.PluginInMemoryTest.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MicroStore.Shipping.PluginInMemoryTest
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(ShippingApplicationAbstractionModule))]
    public class PluginInMemoryModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ShippingInMemoryDbContext>(cfg =>
            {
                cfg.AddDefaultRepositories(true);
                cfg.AddRepository<Shipment, ShipmentRepository>();
            });

            Configure<AbpDbContextOptions>(cfg =>
            {

                cfg.UseInMemoryDb("plugin_system_testdb");

            });

            Configure<AbpUnitOfWorkOptions>(cfg =>
            {
                cfg.IsTransactional = false;
            });


            Configure<AbpUnitOfWorkDefaultOptions>(cfg =>
            {
                cfg.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;

            });

        }
    }
}