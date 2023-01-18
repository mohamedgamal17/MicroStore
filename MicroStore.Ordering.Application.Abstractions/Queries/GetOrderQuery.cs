using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Application.Abstractions.Dtos;

namespace MicroStore.Ordering.Application.Abstractions.Queries
{
    public class GetOrderQuery : IQuery<OrderDto>
    {
        public Guid OrderId { get; set; }
    }
}
