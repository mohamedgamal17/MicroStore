using MassTransit;
using MicroStore.Payment.Api.Services;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Payment.Api.Consumers
{
    public class AcceptPaymentIntegrationEventConsumer : IConsumer<AcceptPaymentIntegationEvent>
    {

        private readonly IPaymentService _paymentService;

        private readonly ILogger<AcceptPaymentIntegrationEventConsumer> _logger;

        public AcceptPaymentIntegrationEventConsumer(IPaymentService paymentService, ILogger<AcceptPaymentIntegrationEventConsumer> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AcceptPaymentIntegationEvent> context)
        {
            try
            {

                await _paymentService.CapturePayment(context.Message.TransactionId);

                await context.Publish(new PaymentAccepetedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNumber = context.Message.OrderNumber,
                    TransactionId = context.Message.TransactionId,
                    PaymentAcceptedDate = DateTime.UtcNow
                });

            }catch(Exception ex)
            {
                _logger.LogException(ex);

                await _paymentService.RefundPayment(context.Message.TransactionId);

                await context.Publish(new PaymentRejectedIntegrationEvent
                {
                    OrderId = context.Message.OrderId,
                    OrderNumber = context.Message.OrderNumber,
                    TransactionId = context.Message.TransactionId,
                    FaultDate = DateTime.UtcNow
                });
            }
        }
    }
}
