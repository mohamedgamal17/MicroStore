using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Inventory.Application.Abstractions.Commands;
namespace MicroStore.Inventory.Application.Consumers
{
    public class ProductUpdateIntegrationEventConsumer : IConsumer<ProductUpdatedIntegerationEvent>
    {
        private readonly ILocalMessageBus _localMessageBus;

        public ProductUpdateIntegrationEventConsumer(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }

        public Task Consume(ConsumeContext<ProductUpdatedIntegerationEvent> context)
        {
            return _localMessageBus.Send(new UpdateProdutCommand
            {
                ExternalProductId = context.Message.ProductId,
                Sku = context.Message.Sku,
                Name = context.Message.Name,
                Thumbnail = context.Message.Thumbnail
            });
        }
    }
}
