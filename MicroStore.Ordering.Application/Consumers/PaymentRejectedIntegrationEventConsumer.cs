using MassTransit;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;
namespace MicroStore.Ordering.Application.Consumers
{
    internal class PaymentRejectedIntegrationEventConsumer : IConsumer<PaymentRejectedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<PaymentRejectedIntegrationEvent> context)
        {
            return context.Publish(new OrderPaymentRejectedEvent
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                FaultDate = context.Message.FaultDate
            });
        }
    }
}
