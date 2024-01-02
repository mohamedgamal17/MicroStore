using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Paging.Params;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public interface IProductQueryService
    {
        Task<Result<PagedResult<ElasticProduct>>> ListAsync(ProductListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<List<ElasticProductImage>>> ListProductImagesAsync(string productid, CancellationToken cancellationToken = default);
        Task<Result<ElasticProduct>> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<Result<ElasticProductImage>> GetProductImageAsync(string productId, string imageId, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ElasticProduct>>> SearchByImage(ProductSearchByImageQueryModel model, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<ElasticProduct>>> GetUserRecommendation(string userId, PagingQueryParams queryParams, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<ElasticProduct>>> GetSimilarItems(string productId, PagingQueryParams queryParams, CancellationToken cancellationToken = default);
    }


}
