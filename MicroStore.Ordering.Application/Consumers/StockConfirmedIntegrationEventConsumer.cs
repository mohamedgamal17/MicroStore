using MassTransit;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Events;
namespace MicroStore.Ordering.Application.Consumers
{
    public class StockConfirmedIntegrationEventConsumer : IConsumer<StockConfirmedIntegrationEvent>
    {

        public Task Consume(ConsumeContext<StockConfirmedIntegrationEvent> context)
        {
            return context.Publish(new OrderApprovedEvent
            {
                OrderId = Guid.Parse(context.Message.ExternalOrderId),
                OrderNumber = context.Message.OrderNumber
            });
        }
    }
}
