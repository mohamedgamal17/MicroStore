using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MicroStore.Catalog.Application
{
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

            ConfigureMassTransit(context.Services, configuration);
        }

        private void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {

            services.AddMassTransit(transitConfig =>
            {
                transitConfig.AddConsumers(Assembly.GetExecutingAssembly());


                transitConfig.UsingRabbitMq((ctx, rabbitConfig) =>
                {
                    rabbitConfig.Host(configuration.GetValue<string>("MassTransitConfig:Host"), cfg =>
                    {
                        cfg.Username(configuration.GetValue<string>("MassTransitConfig:UserName"));
                        cfg.Password(configuration.GetValue<string>("MassTransitConfig:Password"));

                    });

                });

            });
        }
    }
}