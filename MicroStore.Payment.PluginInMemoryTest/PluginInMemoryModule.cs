using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Domain;
using MicroStore.Payment.PluginInMemoryTest.EntityFramework;
using MicroStore.Payment.PluginInMemoryTest.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MicroStore.Payment.PluginInMemoryTest
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpAutofacModule))]
    public class PluginInMemoryModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PaymentInMemoryDbContext>(cfg =>
            {
                cfg.AddDefaultRepositories(true);
                cfg.AddRepository<PaymentRequest, PaymentRequestRepository>();
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