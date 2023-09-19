using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductReviewService
    {
        const string BASE_URL = "/catalog/products/{0}/productreviews";

        const string BASE_URL_WITH_ID = "/catalog/products/{0}/productreviews/{1}";

        private readonly MicroStoreClinet _microStoreClient;

        public ProductReviewService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }

        public async Task<ProductReview> CreateAsync(string productId,ProductReviewRequestOption request, CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL, productId);

           return await _microStoreClient.MakeRequest<ProductReview>(path, HttpMethod.Post, request, cancellationToken);
        }

        public async Task<ProductReview> UpdateAsync(string productId,string productReviewId,ProductReviewRequestOption request, CancellationToken cancellationToken = default)
        {
            var path = string.Format(BASE_URL_WITH_ID, productId, productReviewId);

            return await _microStoreClient.MakeRequest<ProductReview>(path,HttpMethod.Put, request, cancellationToken);
        }

        public async Task<ProductReview> ReplayAsync(string productId , string reveiwId, ReplayOnProductReviewRequestOption request, CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL_WITH_ID, productId, reveiwId);

            return await _microStoreClient.MakeRequest<ProductReview>(path, HttpMethod.Post, request, cancellationToken);
        }


        public async Task<PagedList<ProductReview>> ListAsync(string productId, ProductReviewListRequestOption request, CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL, productId);

            return await _microStoreClient.MakeRequest<PagedList<ProductReview>>(path, HttpMethod.Get, request ,cancellationToken);
        }


        public async Task<ProductReview> GetAsync(string productId ,string reveiwId, CancellationToken cancellationToken)
        {
            string path = string.Format(BASE_URL_WITH_ID, productId, reveiwId);

            return await _microStoreClient.MakeRequest<ProductReview>(path, HttpMethod.Get, cancellationToken: cancellationToken);
        }
    }
}
