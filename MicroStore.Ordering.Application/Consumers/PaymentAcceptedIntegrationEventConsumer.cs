using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Consumers
{
    internal class PaymentAcceptedIntegrationEventConsumer : IConsumer<PaymentAccepetedIntegrationEvent>
    {

        public async Task Consume(ConsumeContext<PaymentAccepetedIntegrationEvent> context)
        {
            await context.Publish(new OrderPaymentAcceptedEvent
            {
                OrderId = context.Message.OrderId,
                OrderNubmer = context.Message.OrderNumber,
                TransactionId = context.Message.TransactionId,
                PaymentAcceptedDate = context.Message.PaymentAcceptedDate
            });
        }
    }
}
