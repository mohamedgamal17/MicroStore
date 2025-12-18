using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public interface IProductQueryService
    {
        Task<Result<PagedResult<ProductDto>>> ListAsync(ProductListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<List<ProductDto>>> ListByIdsAsync(List<string> ids, CancellationToken cancellationToken = default);
        Task<Result<List<ProductImageDto>>> ListProductImagesAsync(string productid, CancellationToken cancellationToken = default);
        Task<Result<ProductDto>> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<ProductImageDto>> GetProductImageAsync(string productId, string imageId, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ProductDto>>> SearchByImage(ProductSearchByImageQueryModel model, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ProductDto>>> GetUserRecommendation(string userId, PagingQueryParams queryParams, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<ProductDto>>> GetSimilarItems(string productId, PagingQueryParams queryParams, CancellationToken cancellationToken = default);
    }


}
