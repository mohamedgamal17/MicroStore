using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Payment.Application.Domain.Events;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;

namespace MicroStore.Payment.Application.EventHandlers
{
    public class PaymentCompletedEventHandler : ILocalEventHandler<PaymentCompletedEvent>, ITransientDependency
    {
        private readonly IPublishEndpoint _publisherEndPoint;

        private readonly ILogger<PaymentCompletedEventHandler> _logger;
        public PaymentCompletedEventHandler(IPublishEndpoint publisherEndPoint, ILogger<PaymentCompletedEventHandler> logger)
        {
            _publisherEndPoint = publisherEndPoint;
            _logger = logger;
        }

        public async Task HandleEventAsync(PaymentCompletedEvent eventData)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {

                _logger.LogDebug("Handling local event : {EventName}", nameof(PaymentCompletedEvent));
            }

            PaymentCompletedIntegrationEvent paymentCompletedIntegrationEvent = new PaymentCompletedIntegrationEvent
            {
                PaymentId = eventData.PaymentId.ToString(),
                OrderId = eventData.OrderId,
                OrderNumber = eventData.OrderNumber,
                PaymentCompletionDate = eventData.CapturedAt
            };

            await _publisherEndPoint.Publish(paymentCompletedIntegrationEvent);
        }
    }
}
