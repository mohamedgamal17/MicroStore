using MassTransit;
using MicroStore.Inventory.Application.Models;
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
            var model = new OrderStockModel
            {
                Items = MapeOrderItems(context.Message.Items)
            }; 


            await _orderCommandService.ReleaseOrderStockAsync(model);
        }


        private List<OrderItemModel> MapeOrderItems(List<MicroStore.Inventory.IntegrationEvents.Models.OrderItemModel> products)
        {
            return products.Select(x => new OrderItemModel
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();
        }
    }
}
