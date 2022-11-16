using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Payment.Application.Domain.Events;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.EventBus;

namespace MicroStore.Payment.Application.EventHandlers
{
    public class PaymentFaildEventHandler : ILocalEventHandler<PaymentFaildEvent>
    {

        private readonly IPublishEndpoint _publishEndpoint;

        private readonly ILogger<PaymentFaildEvent> _logger;

        public PaymentFaildEventHandler(IPublishEndpoint publishEndpoint, ILogger<PaymentFaildEvent> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task HandleEventAsync(PaymentFaildEvent eventData)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Handling local event : {EventName}", nameof(PaymentFaildEvent));
            }

            PaymentFaildIntegrationEvent paymentRejectedIntegrationEvent = new PaymentFaildIntegrationEvent
            {
                PaymentId = eventData.PaymentId.ToString(),
                OrderId = eventData.OrderId,
                OrderNumber = eventData.OrderNumber,
                CustomerId = eventData.CustomerId,
                FaultDate = eventData.FaultDate
            };

            await _publishEndpoint.Publish(paymentRejectedIntegrationEvent);
        }
    }
}
