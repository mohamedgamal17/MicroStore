using MicroStore.Shipping.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using MassTransit;
using MicroStore.Ordering.IntegrationEvents;
using Volo.Abp.Threading;
namespace MicroStore.Shipping.Application.EventHandlers
{
    public class ShipmentCreatedEventHandler : ILocalEventHandler<EntityCreatedEventData<Shipment>>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private ICancellationTokenProvider _cancellationTokenProvider;

        public ShipmentCreatedEventHandler(IPublishEndpoint publishEndpoint, ICancellationTokenProvider cancellationTokenProvider)
        {
            _publishEndpoint = publishEndpoint;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public Task HandleEventAsync(EntityCreatedEventData<Shipment> eventData)
        {
            return _publishEndpoint.Publish(new FullfillOrderIntegrationEvent
            {
                OrderId = Guid.Parse(eventData.Entity.OrderId),
                ShipmentId = eventData.Entity.Id.ToString(),
            }, _cancellationTokenProvider.Token);
        }
    }
}
