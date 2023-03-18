﻿using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Volo.Abp.AutoMapper;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace MicroStore.Inventory.Application
{
    [DependsOn(typeof(AbpFluentValidationModule),
        typeof(AbpAutoMapperModule))]
    public class InventoryApplicationModule  : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<InventoryApplicationModule>();
            });

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
                        cfg.Username(configuration.GetValue<string>("MassTransitConfig:UserName"));
                        cfg.Password(configuration.GetValue<string>("MassTransitConfig:Password"));
                    });

                    rabbitMqbusConig.ConfigureEndpoints(context);
                });

            });
        }

    }
}
