
using MassTransit;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Consumers
{
    public class OrderPaymentEventConsumer :
        IConsumer<PaymentAccepetedIntegrationEvent>,
        IConsumer<PaymentFaildIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<PaymentAccepetedIntegrationEvent> context)
        {
            await context.Publish(new OrderPaymentAcceptedEvent
            {
                OrderId = Guid.Parse(context.Message.OrderId),
                PaymentId = context.Message.PaymentId,
            });
        }

        public async Task Consume(ConsumeContext<PaymentFaildIntegrationEvent> context)
        {
            await context.Publish(new OrderPaymentFaildEvent
            {
                OrderId = context.Message.OrderId,
                FaultDate = context.Message.FaultDate,
                FaultReason = context.Message.FaultReason

            });
        }
    }
}
