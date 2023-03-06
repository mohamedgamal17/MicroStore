using MassTransit;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Inventory.Application.Models;
using MicroStore.Inventory.Application.Products;
using System.Xml.Linq;

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

            await _productCommandService.UpdateAsync(productModel);
        }
    }
}
