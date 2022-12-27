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
                OrderId = Guid.Parse(context.Message.ExternalOrderId),
                OrderNumber =  context.Message.OrderNumber,
                Details = context.Message.Details
            });
        }
    }
}
