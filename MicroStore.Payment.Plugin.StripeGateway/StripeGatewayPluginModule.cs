using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Plugin.StripeGateway.Config;
using Stripe;
using Volo.Abp.Modularity;

namespace MicroStore.Payment.Plugin.StripeGateway
{
    public class StripeGatewayPluginModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();

            Configure<StripePaymentOption>(config);

            context.Services.AddTransient<PaymentIntentService>();


        }
    }
}
