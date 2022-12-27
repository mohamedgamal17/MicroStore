using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.IntegrationEvents;
using MicroStore.Inventory.Application.Abstractions.Commands;
namespace MicroStore.Inventory.Application.Consumers
{
    public class ProductCreatedIntegrationEventConsumer : IConsumer<ProductCreatedIntegrationEvent>
    {
        private ILocalMessageBus _localMessageBus;

        public ProductCreatedIntegrationEventConsumer(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }

        public Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
        {
            return _localMessageBus.Send(new DispatchProductCommand
            {
                ExternalProductId = context.Message.ProductId,
                Sku = context.Message.Sku,
                Name = context.Message.Name,
                Thumbnail = context.Message.Thumbnail
            });
        }
    }
}
