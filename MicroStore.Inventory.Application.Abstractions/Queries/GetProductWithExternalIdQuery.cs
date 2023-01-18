using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetProductWithExternalIdQuery : IQuery<ProductDto>
    {
        public string  ExternalProductId { get; set; }
    }
}
