using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Shipping.IntegrationEvent;

namespace MicroStore.Ordering.Application.Consumers
{
    public class ShippmentCreatedIntegrationEventConsumer : IConsumer<ShippmentCreatedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<ShippmentCreatedIntegrationEvent> context)
        {
            return context.Publish(new OrderShippmentCreatedEvent
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                ShippmentId = context.Message.ShippmentId
            });
        }
    }
}
