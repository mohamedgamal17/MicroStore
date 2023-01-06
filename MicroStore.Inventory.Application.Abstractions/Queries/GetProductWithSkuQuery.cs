using MicroStore.BuildingBlocks.InMemoryBus.Contracts;

namespace MicroStore.Inventory.Application.Abstractions.Queries
{
    public class GetProductWithSkuQuery : IQuery
    {
        public string  Sku { get; set; }
    }
}
