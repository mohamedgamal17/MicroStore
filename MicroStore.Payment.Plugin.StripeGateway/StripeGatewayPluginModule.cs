using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Application.Abstractions;
using Stripe;
using Stripe.Checkout;
using Volo.Abp.Modularity;
namespace MicroStore.Payment.Plugin.StripeGateway
{
    [DependsOn(typeof(PaymentApplicationAbstractionModule))]
    public class StripeGatewayPluginModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
        
            context.Services.AddTransient<PaymentIntentService>();

            context.Services.AddTransient<SessionService>();

            context.Services.AddTransient<RefundService>();

        }
    }
}
