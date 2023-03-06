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
            var productModel = new ProductModel
            {
                ProductId = context.Message.ProductId,
                Sku = context.Message.Sku,
                Name = context.Message.Name,
            };

            if (context.Message.ProductImages != null && context.Message.ProductImages.Count > 0)
            {

                productModel.Thumbnail = context.Message.ProductImages.OrderBy(x => x.DisplayOrder).First().ImageLink;
            }

            await _productCommandService.CreateAsync(productModel);
        }
    }
}
