using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetOrderWithExternalIdQuery : IQuery
    {
        public string ExternalOrderId { get; set; }
    }
}
