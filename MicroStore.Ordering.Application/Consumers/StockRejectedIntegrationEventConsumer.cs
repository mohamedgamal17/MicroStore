using MassTransit;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Events;
namespace MicroStore.Ordering.Application.Consumers
{
    public class StockRejectedIntegrationEventConsumer : IConsumer<StockRejectedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<StockRejectedIntegrationEvent> context)
        {
            return context.Publish(new OrderStockRejectedEvent
            {
                OrderId = context.Message.OrderId,
                Details = context.Message.Details
            });
        }
    }
}
