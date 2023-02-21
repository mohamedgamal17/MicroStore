using MassTransit;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.IntegrationEvents;
namespace MicroStore.Payment.Application.Consumers
{
    public class RefundPaymentRequestIntegrationEventConsumer : IConsumer<RefundPaymentIntegrationEvent>
    {

        private readonly IPaymentRequestCommandService _paymentRequestCommandService;

        public RefundPaymentRequestIntegrationEventConsumer(IPaymentRequestCommandService paymentRequestCommandService)
        {
            _paymentRequestCommandService = paymentRequestCommandService;
        }



        public async Task Consume(ConsumeContext<RefundPaymentIntegrationEvent> context)
        {

           await  _paymentRequestCommandService.RefundPaymentAsync(context.Message.PaymentId);

        }
    }
}
