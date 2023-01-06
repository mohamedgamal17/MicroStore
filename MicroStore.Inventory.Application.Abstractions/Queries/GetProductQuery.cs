using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetProductQuery : IQuery
    {
        public Guid ProductId { get; set; }
    }
}
