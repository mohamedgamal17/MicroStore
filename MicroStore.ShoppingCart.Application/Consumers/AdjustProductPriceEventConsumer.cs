using MassTransit;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace MicroStore.ShoppingCart.Application.Consumers
{
    public class AdjustProductPriceEventConsumer : IConsumer<AdjustProductPriceIntegrationEvent>
         , IUnitOfWorkEnabled
    {

        private readonly IRepository<Product> _productRepository;

        public AdjustProductPriceEventConsumer(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Consume(ConsumeContext<AdjustProductPriceIntegrationEvent> context)
        {
            var product = await _productRepository.SingleAsync(x => x.Id == context.Message.ProductId);
            product.Price = context.Message.Price;

            await _productRepository.UpdateAsync(product);
        }
    }
}
