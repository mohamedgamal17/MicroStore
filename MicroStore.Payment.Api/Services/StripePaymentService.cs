using MicroStore.Payment.Api.Dtos;
using MicroStore.Payment.Api.Models;
using Stripe;
using Volo.Abp.DependencyInjection;

namespace MicroStore.Payment.Api.Services
{
    [ExposeServices(typeof(IPaymentService),IncludeDefaults = true)]
    public class StripePaymentService : IPaymentService , ITransientDependency
    {

        private readonly PaymentIntentService _paymentIntentService;

        public StripePaymentService()
        {
            _paymentIntentService = new PaymentIntentService();

        }

        public Task CapturePayment(string transactionId)
        {
            return _paymentIntentService.CaptureAsync(transactionId);
        }

        public async Task<PaymentDto> CreatePayment(CreatePaymentModel model)
        {
            PaymentIntentCreateOptions paymentIntentCreateOptions = new PaymentIntentCreateOptions
            {
                Amount = Convert.ToInt64(model.Amount * 100),
                CaptureMethod = "manual",
                Currency = "usd",
                PaymentMethodTypes = new List<string>
                {
                    "card",
                }
            };

            var response = await _paymentIntentService.CreateAsync(paymentIntentCreateOptions);

            return new PaymentDto
            {
                Amount = response.Amount,
                TransactionId = response.Id
            };
        }

        public Task RefundPayment(string transactionId)
        {
            PaymentIntentCancelOptions paymentIntentCancelOptions = new PaymentIntentCancelOptions
            {
                CancellationReason = "Unkown"
            };

            return _paymentIntentService.CancelAsync(transactionId, paymentIntentCancelOptions);
        }
    }
}
