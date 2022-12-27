using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;

namespace MicroStore.Ordering.Application.Consumers
{
    public class FullfillOrderIntegrationEventConsumer : IConsumer<FullfillOrderIntegrationEvent>
    {
        public Task Consume(ConsumeContext<FullfillOrderIntegrationEvent> context)
        {
            return context.Publish(new OrderFulfillmentCompletedEvent
            {
                OrderId = context.Message.OrderId,
                ShipmentId = context.Message.ShipmentId,
              });
        }
    }
}
