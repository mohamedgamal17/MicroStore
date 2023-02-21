using MassTransit;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Products;

namespace MicroStore.Inventory.Application.Consumers
{
    public class ProductUpdateIntegrationEventConsumer : IConsumer<ProductUpdatedIntegerationEvent>
    {


        private readonly IProductCommandService _productCommandService;

        public ProductUpdateIntegrationEventConsumer(IProductCommandService productCommandService)
        {
            _productCommandService = productCommandService;
        }

        public async Task Consume(ConsumeContext<ProductUpdatedIntegerationEvent> context)
        {
            await _productCommandService.UpdateAsync(new ProductModel
            {
                ProductId = context.Message.ProductId,
                Sku = context.Message.Sku,
                Name = context.Message.Name,
                Thumbnail = context.Message.Thumbnail

            });
        }
    }
}
