using MassTransit;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Products;

namespace MicroStore.Inventory.Application.Consumers
{
    public class ProductCreatedIntegrationEventConsumer : IConsumer<ProductCreatedIntegrationEvent>
    {
        private readonly IProductCommandService _productCommandService;


        public ProductCreatedIntegrationEventConsumer(IProductCommandService productCommandService)
        {
            _productCommandService = productCommandService;
        }

        public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
        {
            await _productCommandService.CreateAsync(new ProductModel
            {
                ProductId = context.Message.ProductId,
                Sku = context.Message.Sku,
                Name = context.Message.Name,
                Thumbnail = context.Message.Thumbnail

            });
           
        }
    }
}
