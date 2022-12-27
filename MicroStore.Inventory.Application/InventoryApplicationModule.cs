using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Inventory.Application.Abstractions;
using System.Reflection;
using Volo.Abp.Modularity;

namespace MicroStore.Inventory.Application
{
    [DependsOn(typeof(InventoryAbstractionModule))]
    public class InventoryApplicationModule  : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            ConfigureMassTranstit(context.Services, context.Services.GetConfiguration());

        }


        private void ConfigureMassTranstit(IServiceCollection services , IConfiguration configuration)
        {
            services.AddMassTransit(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(Assembly.GetExecutingAssembly());

             
                busRegisterConfig.UsingRabbitMq((context, rabbitMqbusConig) =>
                {

                    rabbitMqbusConig.Host(configuration.GetValue<string>("MassTransitConfig:Host"), cfg =>
                    {
                        cfg.Username("MassTransitConfig:UserName");
                        cfg.Password("MassTransitConfig:Password");
                    });

                    rabbitMqbusConig.ConfigureEndpoints(context);
                });

            });
        }

    }
}
