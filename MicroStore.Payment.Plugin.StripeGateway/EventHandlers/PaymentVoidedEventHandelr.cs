using Microsoft.Extensions.Options;
using MicroStore.Payment.Domain.Shared.Events;
using MicroStore.Payment.Plugin.StripeGateway.Config;
using MicroStore.Payment.Plugin.StripeGateway.Consts;
using Stripe;
using Volo.Abp.EventBus;

namespace MicroStore.Payment.Plugin.StripeGateway.EventHandlers
{
    public class PaymentVoidedEventHandelr : ILocalEventHandler<PaymentVoidedEvent>
    {

        private readonly PaymentIntentService _paymentIntentService;

        private readonly StripePaymentOption _stripePaymentOption;

        public PaymentVoidedEventHandelr(PaymentIntentService paymentIntentService, IOptions<StripePaymentOption> stripePaymentOption)
        {
            _paymentIntentService = paymentIntentService;
            _stripePaymentOption = stripePaymentOption.Value;
        }

        public Task HandleEventAsync(PaymentVoidedEvent eventData)
        {
            if(eventData.PaymentGatewayApi != StripePaymentDefault.Provider)
            {
                return Task.CompletedTask;
            }

            PaymentIntentCancelOptions paymentIntentCancelOptions = new PaymentIntentCancelOptions
            {
                CancellationReason = "OrderFault"
            };


            RequestOptions requestOptions = new RequestOptions { ApiKey = _stripePaymentOption.ApiKey };

            return _paymentIntentService.CancelAsync(eventData.TransactionId,paymentIntentCancelOptions, requestOptions);
        }
    }
}
