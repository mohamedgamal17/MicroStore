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

            ConfigureMassTransit(context.Services, configuration);
        }

        private void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = services.GetSingletonInstance<ApplicationSettings>(); 

            services.AddMassTransit(transitConfig =>
            {
                transitConfig.AddConsumers(Assembly.GetExecutingAssembly());


                transitConfig.UsingRabbitMq((ctx, rabbitConfig) =>
                {
                    rabbitConfig.Host(configuration.GetValue<string>(appSettings.MassTransit.Host), cfg =>
                    {
                        cfg.Username(configuration.GetValue<string>(appSettings.MassTransit.UserName));
                        cfg.Password(configuration.GetValue<string>(appSettings.MassTransit.Password));

                    });

                    rabbitConfig.ConfigureEndpoints(ctx);

                });

            });
        }
    }
}