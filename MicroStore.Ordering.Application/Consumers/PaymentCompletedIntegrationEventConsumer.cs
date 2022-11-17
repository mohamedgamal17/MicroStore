using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Consumers
{
    public class PaymentCompletedIntegrationEventConsumer : IConsumer<PaymentCompletedIntegrationEvent>
    {

        public async Task Consume(ConsumeContext<PaymentCompletedIntegrationEvent> context)
        {
            await context.Publish(new OrderPaymentCompletedEvent
            {
                OrderId = context.Message.OrderId,
                OrderNubmer = context.Message.OrderNumber,
                TransactionId = context.Message.PaymentId,
                PaymentAcceptedDate = context.Message.PaymentCompletionDate
            });
        }
    }
}
