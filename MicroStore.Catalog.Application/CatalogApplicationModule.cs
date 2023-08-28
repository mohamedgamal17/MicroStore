using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Domain;
using MicroStore.Catalog.Domain.Configuration;
using System.Data;
using System.Reflection;
using Volo.Abp.AutoMapper;
using Volo.Abp.EventBus;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
namespace MicroStore.Catalog.Application
{
    [DependsOn(
        typeof(CatalogDomainModule),
        typeof(AbpEventBusModule),
        typeof(AbpAutoMapperModule), 
        typeof(AbpFluentValidationModule))]
    public class CatalogApplicationModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpUnitOfWorkDefaultOptions>(opt =>
            {
                opt.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
                opt.IsolationLevel = IsolationLevel.ReadCommitted;
            });

            Configure<AbpAutoMapperOptions>(opt =>
            {
               opt.AddMaps<CatalogApplicationModule>();

            });


            var configuration = context.Services.GetConfiguration();
        }

    
    }
}