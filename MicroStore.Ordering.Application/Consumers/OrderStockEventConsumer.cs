using MassTransit;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Application.StateMachines;
namespace MicroStore.Ordering.Application.Consumers
{

    public class OrderStockEventConsumer :
        IConsumer<StockConfirmedIntegrationEvent>,
        IConsumer<StockRejectedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<StockConfirmedIntegrationEvent> context)
        {
            await context.Publish(new OrderApprovedEvent
            {
                OrderId = Guid.Parse(context.Message.ExternalOrderId),
                OrderNumber = context.Message.OrderNumber
            });
        }

        public async Task Consume(ConsumeContext<StockRejectedIntegrationEvent> context)
        {
            await context.Publish(new OrderStockRejectedEvent
            {
                OrderId = Guid.Parse(context.Message.ExternalOrderId),
                OrderNumber = context.Message.OrderNumber,
                Details = context.Message.Details
            });
        }
    }

  
}
