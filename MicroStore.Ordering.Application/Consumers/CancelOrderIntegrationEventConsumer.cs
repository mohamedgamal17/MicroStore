using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;
namespace MicroStore.Ordering.Application.Consumers
{
    public class CancelOrderIntegrationEventConsumer : IConsumer<CancelOrderIntegrationEvent>
    {
        public Task Consume(ConsumeContext<CancelOrderIntegrationEvent> context)
        {
            return context.Publish(new OrderCancelledEvent
            {
                OrderId = context.Message.OrderId,
                Reason = context.Message.Reason,
                CancellationDate = context.Message.CancellationDate,
            });
        }
    }
}
