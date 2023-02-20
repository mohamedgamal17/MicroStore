using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
namespace MicroStore.Catalog.Application.Products
{
    public interface IProductQueryService
    {
        Task<UnitResultV2<PagedResult<ProductListDto>>> ListAsync(PagingAndSortingQueryParams queryParams,CancellationToken cancellationToken = default);
        Task<UnitResultV2<ProductDto>> GetAsync(string id , CancellationToken cancellationToken = default );
    }


}
