using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;

namespace MicroStore.Ordering.Application.Consumers
{
    internal class ConfirmOrderIntegrationEventConsumer : IConsumer<ConfirmOrderIntegrationEvent>
    {
        public Task Consume(ConsumeContext<ConfirmOrderIntegrationEvent> context)
        {
            return context.Publish(new OrderConfirmedEvent
            {
                OrderId = context.Message.OrderId,
                ConfirmationDate = context.Message.ConfirmationDate
            });
        }
    }
}
