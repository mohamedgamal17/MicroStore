using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions;
using System.Data;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
namespace MicroStore.Catalog.Application
{
    [DependsOn(
        typeof(CatalogApplicationAbstractionModule),
        typeof(AbpEventBusModule))]
    public class CatalogApplicationModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpUnitOfWorkDefaultOptions>(opt =>
            {
                opt.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
                opt.IsolationLevel = IsolationLevel.ReadCommitted;
            });

            var configuration = context.Services.GetConfiguration();
        }

    
    }
}