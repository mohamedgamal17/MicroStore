using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;
namespace MicroStore.Ordering.Application.Consumers
{
    public class PaymentFaildIntegrationEventConsumer : IConsumer<PaymentFaildIntegrationEvent>
    {
        public Task Consume(ConsumeContext<PaymentFaildIntegrationEvent> context)
        {
            return context.Publish(new OrderPaymentFaildEvent
            {
                OrderId = context.Message.OrderId,
                FaultDate = context.Message.FaultDate,
                FaultReason = context.Message.FaultReason
                
            });
        }
    }
}
