using MassTransit;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace MicroStore.ShoppingCart.Application.Consumers
{
    public class AdjustProductNameEventConsumer : IConsumer<AdjustProductNameIntegrationEvent>
    {
        private readonly IRepository<Product> _productRepository;

        public AdjustProductNameEventConsumer(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [UnitOfWork]
        public async Task Consume(ConsumeContext<AdjustProductNameIntegrationEvent> context)
        {
            Product product = await _productRepository.SingleAsync(x => x.Id == context.Message.ProductId);

            product.Name = context.Message.Name;

            await _productRepository.UpdateAsync(product);
        }
    }
}
