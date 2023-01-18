using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetOrderWithExternalIdQuery : IQuery<OrderDto>
    {
        public string ExternalOrderId { get; set; }
    }
}
