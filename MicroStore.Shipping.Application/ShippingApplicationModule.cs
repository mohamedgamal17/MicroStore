using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction;
using MicroStore.Shipping.Domain.Entities;
using System.Reflection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace MicroStore.Shipping.Application
{
    [DependsOn(typeof(ShippingApplicationAbstractionModule),
        typeof(AbpDddApplicationModule))]
    public class ShippingApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            ConfigureShippingLocation(config);

            ConfigureMassTransit(context.Services, config);
        }


        private void ConfigureShippingLocation(IConfiguration configuration)
        {
            Configure<ShippingSettings>(cfg =>
            {
                cfg.Location = new AddressSettings
                {
                    Name =configuration.GetValue<string>("ShippingLocation:Name"),
                    CountryCode = configuration.GetValue<string>("ShippingLocation:CountryIsoCode"),
                    State = configuration.GetValue<string>("ShippingLocation:StateIsoCode"),
                    AddressLine1 = configuration.GetValue<string>("ShippingLocation:AddressLine1"),
                    AddressLine2 = configuration.GetValue<string>("ShippingLocation:AddressLine2"),
                    City = configuration.GetValue<string>("ShippingLocation:City"),
                    Zip = configuration.GetValue<string>("ShippingLocation:Zip"),
                    PostalCode = configuration.GetValue<string>("ShippingLocation:PostalCode")
                };
            });
        }


        private void ConfigureMassTransit(IServiceCollection services , IConfiguration configuration)
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

                    rabbitConfig.ConfigureEndpoints(ctx);

                });

            });
        }

     }
}
