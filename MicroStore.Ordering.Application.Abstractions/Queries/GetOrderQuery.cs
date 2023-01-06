using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Ordering.Application.Abstractions.Queries
{
    public class GetOrderQuery : IQuery
    {
        public Guid OrderId { get; set; }
    }
}
