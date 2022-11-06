using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Products.Queries
{
    public class GetProductQuery : IQuery<ProductDto>
    {
        public Guid Id { get; set; }
    }
}
