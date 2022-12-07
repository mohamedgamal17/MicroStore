using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Application.Abstractions.Common;
using MicroStore.Inventory.IntegrationEvents;
namespace MicroStore.Inventory.Application.Consumers
{
    public class ReleaseOrderStockIntegrationEventConsumer : IConsumer<ReleaseOrderStockIntegrationEvent>
    {

        private readonly ILocalMessageBus _localMessageBus;

        public ReleaseOrderStockIntegrationEventConsumer(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }
        public async Task Consume(ConsumeContext<ReleaseOrderStockIntegrationEvent> context)
        {
            await _localMessageBus.Send(new ReleaseOrderStockCommand
            {
                OrderId = context.Message.OrderId,
                OrderNumber = context.Message.OrderNumber,
                Products = MapeProducts(context.Message.Products)
            });
        }

        private List<ProductModel> MapeProducts(List<MicroStore.Inventory.IntegrationEvents.Models.ProductModel> products)
        {
            return products.Select(x => new ProductModel
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity
            }).ToList();
        }
    }
}
