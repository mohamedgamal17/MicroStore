using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using MassTransit;
using MicroStore.Ordering.IntegrationEvents;
using Volo.Abp.Threading;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MicroStore.Shipping.Application.EventHandlers
{
    public class ShipmentCreatedEventHandler : ILocalEventHandler<EntityCreatedEventData<Shipment>> , ITransientDependency
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private ICancellationTokenProvider _cancellationTokenProvider;

        private readonly ILogger<ShipmentCreatedEventHandler> _logger;

        public ShipmentCreatedEventHandler(IPublishEndpoint publishEndpoint, ICancellationTokenProvider cancellationTokenProvider, ILogger<ShipmentCreatedEventHandler> logger)
        {
            _publishEndpoint = publishEndpoint;
            _cancellationTokenProvider = cancellationTokenProvider;
            _logger = logger;
        }

        public Task HandleEventAsync(EntityCreatedEventData<Shipment> eventData)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("shipment entity is : {shipment}", eventData.Entity);
            }

            return _publishEndpoint.Publish(new FullfillOrderIntegrationEvent
            {
                OrderId = Guid.Parse(eventData.Entity.OrderId),
                ShipmentId = eventData.Entity.Id.ToString(),
            }, _cancellationTokenProvider.Token);
        }
    }
}
