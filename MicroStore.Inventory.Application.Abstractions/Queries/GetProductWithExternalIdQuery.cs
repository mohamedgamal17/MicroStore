using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetProductWithExternalIdQuery : IQuery
    {
        public string  ExternalProductId { get; set; }
    }
}
