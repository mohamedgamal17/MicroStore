using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Dtos;

namespace MicroStore.Catalog.Application.Products
{
    public class GetProductListQuery : PagingAndSortingQueryParams, IQuery<PagedResult<ProductListDto>>
    {

    }
    public class GetProductQuery : IQuery<ProductDto>
    {
        public Guid Id { get; set; }
    }
}
