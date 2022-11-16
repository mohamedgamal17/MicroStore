using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Payment.Application.Domain.Events;
using MicroStore.Payment.IntegrationEvents;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
namespace MicroStore.Payment.Application.EventHandlers
{
    public class PaymentCreatedEventHandler : ILocalEventHandler<PaymentCreatedEvent>, ITransientDependency
    {

        private readonly IPublishEndpoint _publisherEndpoint;

        private readonly ILogger<PaymentCreatedEventHandler> _logger;

        public PaymentCreatedEventHandler(IPublishEndpoint publisherEndpoint, ILogger<PaymentCreatedEventHandler> logger)
        {
            _publisherEndpoint = publisherEndpoint;
            _logger = logger;
        }

        public async Task HandleEventAsync(PaymentCreatedEvent eventData)
        {

            if (_logger.IsEnabled(LogLevel.Debug))
            {

                _logger.LogDebug("Handling local event : {EventName}", nameof(PaymentCreatedEvent));

            }

            PaymentCreatedIntegrationEvent paymentCreatedIntegrationEvent = new PaymentCreatedIntegrationEvent
            {
                PaymentId = eventData.PaymentId.ToString(),
                OrderId = eventData.OrderId,
                CustomerId = eventData.CustomerId,
                OrderNubmer = eventData.OrderNumber
            };

            await _publisherEndpoint.Publish(paymentCreatedIntegrationEvent);
        }
    }
}
