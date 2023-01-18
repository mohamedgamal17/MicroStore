using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetProductQuery : IQuery<ProductDto>
    {
        public Guid ProductId { get; set; }
    }
}
