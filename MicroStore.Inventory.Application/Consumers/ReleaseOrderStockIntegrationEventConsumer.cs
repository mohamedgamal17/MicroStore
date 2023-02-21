using MassTransit;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.IntegrationEvents;
namespace MicroStore.Inventory.Application.Consumers
{
    public class ReleaseOrderStockIntegrationEventConsumer : IConsumer<ReleaseOrderStockIntegrationEvent>
    {

        private readonly IOrderCommandService _orderCommandService;

        public ReleaseOrderStockIntegrationEventConsumer(IOrderCommandService orderCommandService)
        {
            _orderCommandService = orderCommandService;
        }


        public async Task Consume(ConsumeContext<ReleaseOrderStockIntegrationEvent> context)
        {
            await _orderCommandService.ReleaseOrderStockAsync(context.Message.OrderId);
        }
    }
}
