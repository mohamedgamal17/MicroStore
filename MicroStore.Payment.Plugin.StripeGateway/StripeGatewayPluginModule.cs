using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using Stripe;
using Stripe.Checkout;
using Volo.Abp;
using Volo.Abp.Modularity;
namespace MicroStore.Payment.Plugin.StripeGateway
{
    public class StripeGatewayPluginModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
        
            context.Services.AddTransient<PaymentIntentService>();

            context.Services.AddTransient<SessionService>();

            context.Services.AddTransient<RefundService>();

        }


        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var paymentSystemManager = scope.ServiceProvider.GetRequiredService<IPaymentSystemManager>();

                await paymentSystemManager.TryToCreate(StripePaymentConst.Provider, StripePaymentConst.DisplayName, StripePaymentConst.Image);
            }
        }
    }
}
