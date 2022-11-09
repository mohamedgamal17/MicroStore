using MassTransit;
using MicroStore.Payment.Api.Services;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Payment.Api.Consumers
{
    public class RefundUserIntegrationEventConsumer : IConsumer<RefundUserIntegrationEvent>
    {
        private readonly IPaymentService _paymentService;

        private readonly ILogger<RefundUserIntegrationEventConsumer> _logger;

        public RefundUserIntegrationEventConsumer(IPaymentService paymentService, ILogger<RefundUserIntegrationEventConsumer> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RefundUserIntegrationEvent> context)
        {
            try
            {
                await _paymentService.RefundPayment(context.Message.TransactionId);

            }catch(Exception ex)
            {
                _logger.LogException(ex);
            }
        }
    }
}
