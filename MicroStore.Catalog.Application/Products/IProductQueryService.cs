using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Products;

namespace MicroStore.Catalog.Application.Products
{
    public interface IProductQueryService
    {
        Task<Result<PagedResult<ProductDto>>> ListAsync(PagingAndSortingQueryParams queryParams,CancellationToken cancellationToken = default);
        Task<Result<List<ProductImageDto>>> ListProductImagesAsync(string productid, CancellationToken cancellationToken = default);
        Task<Result<ProductDto>> GetAsync(string id , CancellationToken cancellationToken = default );
        Task<Result<PagedResult<ProductDto>>> SearchAsync(ProductSearchModel model , CancellationToken cancellationToken = default);

    }


}
