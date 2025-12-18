using MicroStore.BuildingBlocks.Utils.Paging;
using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Abstractions.ProductReveiws
{
    public interface IProductReviewService
    {
        Task<Result<ProductReviewDto>> CreateAsync(string productId, CreateProductReviewModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductReviewDto>> UpdateAsync(string productId, string productReviewId, ProductReviewModel model, CancellationToken cancellationToken = default);

        Task<Result<ProductReviewDto>> ReplayAsync(string productId, string productReviewId, ProductReviewReplayModel model, CancellationToken cancellationToken = default);

        Task<Result<Unit>> DeleteAsync(string productId, string productReviewId, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<ElasticProductReview>>> ListAsync(string productId, ProductReviewListQueryModel queryParams, CancellationToken cancellationToken = default);

        Task<Result<ElasticProductReview>> GetAsync(string productId, string productReviewId, CancellationToken cancellationToken = default);
    }
}
