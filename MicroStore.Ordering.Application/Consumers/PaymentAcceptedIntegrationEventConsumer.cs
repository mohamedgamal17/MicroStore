using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Consumers
{
    public class PaymentAcceptedIntegrationEventConsumer : IConsumer<PaymentAccepetedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<PaymentAccepetedIntegrationEvent> context)
        {
            await context.Publish(new OrderPaymentAcceptedEvent
            {
                OrderId = Guid.Parse(context.Message.OrderId),
                PaymentId = context.Message.PaymentId,
            });
        }
    }
}
