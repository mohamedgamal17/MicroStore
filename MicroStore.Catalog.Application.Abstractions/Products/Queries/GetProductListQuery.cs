using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
namespace MicroStore.Catalog.Application.Abstractions.Products.Queries
{
    public class GetProductListQuery : PagingAndSortingQueryParams,  IQuery<PagedResult<ProductListDto>>
    {

    }
}
