using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Api.Dtos;
using MicroStore.Payment.Api.Models;
using MicroStore.Payment.Api.Services;
using MicroStore.TestBase.Extensions;
using Moq;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace MicroStore.Payment.Api.Tests
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class PaymentApiTestModule : AbpModule
    {

        public PaymentApiTestModule()
        {
            
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMassTransitTestHarness(busRegisterConfig =>
            {
                busRegisterConfig.AddConsumers(typeof(PaymentApiModule).Assembly);

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.ConfigureEndpoints(context);
                });
            });

            
        }

    }
}