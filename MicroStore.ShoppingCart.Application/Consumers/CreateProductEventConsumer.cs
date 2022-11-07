using MassTransit;
using Microsoft.Extensions.Logging;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace MicroStore.ShoppingCart.Application.Consumers
{
    public class CreateProductEventConsumer : IConsumer<CreateProductIntegrationEvent>
          , IUnitOfWorkEnabled
    {


        private readonly IRepository<Product> _productRepository;

        private readonly ILogger<CreateProductEventConsumer> _logger;



        public CreateProductEventConsumer(IRepository<Product> productRepository, ILogger<CreateProductEventConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }



        public async Task Consume(ConsumeContext<CreateProductIntegrationEvent> context)
        {
            _logger.LogDebug("Consume ({EventName})", typeof(CreateProductIntegrationEvent).Name);

            Product product = new Product(context.Message.ProductId, context.Message.Name, context.Message.Sku, context.Message.Price);

            await _productRepository.InsertAsync(product);
        }
    }
}
