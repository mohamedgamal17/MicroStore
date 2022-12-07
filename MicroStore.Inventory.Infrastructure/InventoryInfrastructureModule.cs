using Microsoft.Extensions.DependencyInjection;
using MicroStore.Inventory.Application.Abstractions;
using MicroStore.Inventory.Infrastructure.EntityFramework;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MicroStore.Inventory.Infrastructure
{

    [DependsOn(typeof(InventoryAbstractionModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEventBusModule))]
    public  class InventoryInfrastructureModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            context.Services.AddAbpDbContext<InventoyDbContext>(opt =>
            {
                opt.AddDefaultRepositories(true);
            });

            Configure<AbpDbContextOptions>(opt =>
            {
                opt.UseSqlServer(builder => builder.MigrationsAssembly(typeof(InventoyDbContext).Assembly.FullName));
            });

            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Enabled;
                options.IsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            });

        }
    }
}
