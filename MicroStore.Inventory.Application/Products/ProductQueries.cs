using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Inventory.Application.Dtos;

namespace MicroStore.Inventory.Application.Products
{
    public class GetProductListQuery : PagingQueryParams, IQuery<PagedResult<ProductDto>>
    {

    }
    public class GetProductWithExternalIdQuery : IQuery<ProductDto>
    {
        public string ExternalProductId { get; set; }
    }
    public class GetProductWithSkuQuery : IQuery<ProductDto>
    {
        public string Sku { get; set; }
    }
    public class GetProductQuery : IQuery<ProductDto>
    {
        public Guid ProductId { get; set; }
    }
}
