using MassTransit;
using MicroStore.Payment.Api.Models;
using MicroStore.Payment.Api.Services;
using MicroStore.Payment.IntegrationEvents;
using MicroStore.Payment.IntegrationEvents.Responses;

namespace MicroStore.Payment.Api.Consumers
{
    public class CreatePaymentRequestConsumer : IConsumer<CreatePaymentRequest>
    {

        private readonly IPaymentService _paymentService;

        public CreatePaymentRequestConsumer(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task Consume(ConsumeContext<CreatePaymentRequest> context)
        {

            var payment = await _paymentService.CreatePayment(new CreatePaymentModel
            {
                Amount = context.Message.TotalPrice,
            });

            await context.RespondAsync(new PaymentCreatedResponse
            {
                TransactionId = payment.TransactionId,
                Gateway = "Stripe"
            });
            
        }
    }
}
