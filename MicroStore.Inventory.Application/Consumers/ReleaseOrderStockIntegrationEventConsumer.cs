using MassTransit;
using MicroStore.Inventory.Domain.ProductAggregate;
using MicroStore.Inventory.IntegrationEvents;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Inventory.Application.Consumers
{
    public class ReleaseOrderStockIntegrationEventConsumer : IConsumer<ReleaseOrderStockIntegrationEvent>
    {

        private readonly IRepository<Product> _productRepository;

        public ReleaseOrderStockIntegrationEventConsumer(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<ReleaseOrderStockIntegrationEvent> context)
        {
            foreach (var orderItem in context.Message.Products)
            {
                Product product = await _productRepository.SingleAsync(x => x.Id == orderItem.ProductId);

                product.ReleaseStock(orderItem.Quantity);

                await _productRepository.UpdateAsync(product);
            }

        }
    }
}
