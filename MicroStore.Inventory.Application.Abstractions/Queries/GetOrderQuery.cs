using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetOrderQuery : IQuery<OrderDto>
    {
        public Guid OrderId { get; set; }
    }
}
