using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Inventory.Domain;
using MicroStore.Inventory.Domain.Configuration;
using System.Reflection;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace MicroStore.Inventory.Application
{
    [DependsOn(typeof(InventoryDomainModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpAutoMapperModule))]
    public class InventoryApplicationModule  : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<InventoryApplicationModule>();
            });

            ConfigureMassTranstit(context.Services);

        }


        private void ConfigureMassTranstit(IServiceCollection services )
        {
            var appsettings = services.GetSingletonInstance<ApplicationSettings>();

            services.AddMassTransit(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(Assembly.GetExecutingAssembly());

             
                busRegisterConfig.UsingRabbitMq((context, rabbitMqbusConig) =>
                {

                    rabbitMqbusConig.Host(appsettings.MassTransit.Host, cfg =>
                    {
                        cfg.Username(appsettings.MassTransit.UserName);
                        cfg.Password(appsettings.MassTransit.Password);
                    });

                    rabbitMqbusConig.ConfigureEndpoints(context);
                });

            });
        }

    }
}
