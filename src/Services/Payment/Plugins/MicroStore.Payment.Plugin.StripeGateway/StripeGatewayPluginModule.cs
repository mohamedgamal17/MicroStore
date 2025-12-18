using Microsoft.Extensions.DependencyInjection;
using MicroStore.Payment.Domain.Shared;
using MicroStore.Payment.Domain.Shared.Configuration;
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

            var appsettings = context.Services.GetSingletonInstance<ApplicationSettings>();

            Configure<PaymentSystemOptions>(opt =>
            {
                var providerConfiguration = appsettings.PaymentProviders.FindByKey(StripePaymentConst.Provider);

                opt.Systems.Add(PaymentSystem.Create(StripePaymentConst.Provider, StripePaymentConst.DisplayName, StripePaymentConst.Image, typeof(StripePaymentMethodProvider), providerConfiguration));
            });

            context.Services.AddTransient<PaymentIntentService>();

            context.Services.AddTransient<SessionService>();

            context.Services.AddTransient<RefundService>();

        }
    }
}
