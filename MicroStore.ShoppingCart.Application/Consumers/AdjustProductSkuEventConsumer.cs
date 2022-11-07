using MassTransit;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace MicroStore.ShoppingCart.Application.Consumers
{
    public class AdjustProductSkuEventConsumer : IConsumer<AdjustProductSkuIntegrationEvent>
        , IUnitOfWorkEnabled
    {
        private readonly IRepository<Product> _productRepository;

        public AdjustProductSkuEventConsumer(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<AdjustProductSkuIntegrationEvent> context)
        {
            Product product = await _productRepository.SingleAsync(x => x.Id == context.Message.ProductId);

            product.Sku = context.Message.Sku;

            await _productRepository.UpdateAsync(product);
        }
    }
}

