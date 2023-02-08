using MassTransit;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Orders;
using MicroStore.Inventory.IntegrationEvents;
namespace MicroStore.Inventory.Application.Consumers
{
    public class ReleaseOrderStockIntegrationEventConsumer : IConsumer<ReleaseOrderStockIntegrationEvent>
    {

        private readonly ILocalMessageBus _localMessageBus;

        public ReleaseOrderStockIntegrationEventConsumer(ILocalMessageBus localMessageBus)
        {
            _localMessageBus = localMessageBus;
        }
        public async Task Consume(ConsumeContext<ReleaseOrderStockIntegrationEvent> context)
        {
            await _localMessageBus.Send(new ReleaseOrderStockCommand
            {
                ExternalOrderId = context.Message.ExternalOrderId,    
            });
        }      
    }
}
