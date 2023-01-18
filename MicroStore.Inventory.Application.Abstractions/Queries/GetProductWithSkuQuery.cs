using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Inventory.Application.Abstractions.Dtos;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetProductWithSkuQuery : IQuery<ProductDto>
    {
        public string  Sku { get; set; }
    }
}
