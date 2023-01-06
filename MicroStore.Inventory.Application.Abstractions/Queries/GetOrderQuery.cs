using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetOrderQuery : IQuery
    {
        public Guid OrderId { get; set; }
    }
}
