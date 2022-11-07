using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.Security;
using MicroStore.ShoppingCart.Application.Abstraction;
using System.Data;
using System.Reflection;
using Volo.Abp.EventBus;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace MicroStore.ShoppingCart.Application
{
    [DependsOn(typeof(ShoppingCartApplicationAbstractionModule),
        typeof(AbpEventBusModule),
        typeof(MicroStoreSecurityModule))]

    public class ShoppingCartApplicationModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpUnitOfWorkDefaultOptions>(opt =>
            {
                opt.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
                opt.IsolationLevel = IsolationLevel.ReadCommitted;
            });
        }

        private void ConfigureMassTransit(IServiceCollection services ,  IConfiguration configuration)
        {
            services.AddMassTransit(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(Assembly.GetExecutingAssembly());

                busRegisterConfig.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("div", false));

                busRegisterConfig.UsingRabbitMq((ctx, rabbitMqConfig) =>
                {

                    rabbitMqConfig.Host(configuration.GetValue<string>("MassTransitConfig:Host"), cfg =>
                    {
                        cfg.Username(configuration.GetValue<string>("MassTransitConfig:UserName"));
                        cfg.Password(configuration.GetValue<string>("MassTransitConfig:Password"));
                    });

                    rabbitMqConfig.ConfigureEndpoints(ctx);

                });

            });
        }

    }
}