using Microsoft.Extensions.Options;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
using MicroStore.ShoppingGateway.ClinetSdk.Services.Profiling;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductReviewAggregateService :
        INestedListableWithPaging<ProductReviewAggregate, ProductReviewListRequestOption>,
        INestedRetrievable<ProductReviewAggregate>
    {
        private readonly ProductReviewService _productReviewService;
            
        private readonly ProfileService _profileService;

        public ProductReviewAggregateService(ProductReviewService productReviewService, ProfileService profileService)
        {
            _productReviewService = productReviewService;
            _profileService = profileService;
        }
        public async Task<PagedList<ProductReviewAggregate>> ListAsync(string parentId, ProductReviewListRequestOption options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var response = await _productReviewService.ListAsync(parentId, options, requestHeaderOptions, cancellationToken);


            var tasks = response.Items.Select(x => PrepareProductReviewAggregate(x, cancellationToken));

            var aggregates = await Task.WhenAll(tasks);

            var pagedList = new PagedList<ProductReviewAggregate>
            {
                Items = aggregates.ToList(),
                Lenght = response.Lenght,
                Skip = response.Skip,
                TotalCount = response.TotalCount
            };

            return pagedList;
        }

        public async Task<ProductReviewAggregate> GetAsync(string parentId, string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            var response = await _productReviewService.GetAsync(parentId, id, requestHeaderOptions, cancellationToken);

            return await PrepareProductReviewAggregate(response, cancellationToken);
        }
        private async Task<ProductReviewAggregate> PrepareProductReviewAggregate(ProductReview review,  CancellationToken cancellationToken = default)
        {

            var user = await _profileService.GetAsync(review.UserId, cancellationToken: cancellationToken);


            ProductReviewAggregate aggregate = new ProductReviewAggregate
            {
                Id = review.Id,
                ProductId = review.ProductId,
                UserId = review.UserId,
                User = user,
                Rating = review.Rating,
                Title = review.Title,
                ReviewText = review.ReviewText,
                ReplayText = review.ReplayText
            };

            return aggregate;
        }

       
    }
}
