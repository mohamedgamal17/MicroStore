using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductReviewService : Service,
        INestedCreatable<ProductReview, ProductReviewRequestOption>,
        INestedUpdateable<ProductReview, ProductReviewRequestOption>,
        INestedListableWithPaging<ProductReview, ProductReviewListRequestOption>,
        INestedRetrievable<ProductReview>
        
    {
        const string BASE_URL = "/catalog/products/{0}/productreviews";

        const string BASE_URL_WITH_ID = "/catalog/products/{0}/productreviews/{1}";

        private readonly MicroStoreClinet _microStoreClient;

        public ProductReviewService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<ProductReview> CreateAsync(string productId,ProductReviewRequestOption request = null , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL, productId);

           return await MakeRequestAsync<ProductReview>(path, HttpMethod.Post, request, requestHeaderOptions, cancellationToken);
        }

        public async Task<ProductReview> UpdateAsync(string productId,string productReviewId,ProductReviewRequestOption request = null, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            var path = string.Format(BASE_URL_WITH_ID, productId, productReviewId);

            return await MakeRequestAsync<ProductReview>(path,HttpMethod.Put, request, requestHeaderOptions, cancellationToken);
        }

        public async Task<ProductReview> ReplayAsync(string productId , string reveiwId, ReplayOnProductReviewRequestOption request =  null, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL_WITH_ID, productId, reveiwId);

            return await MakeRequestAsync<ProductReview>(path, HttpMethod.Post, request,requestHeaderOptions, cancellationToken);
        }


        public async Task<PagedList<ProductReview>> ListAsync(string productId, ProductReviewListRequestOption request = null,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL, productId);

            return await MakeRequestAsync<PagedList<ProductReview>>(path, HttpMethod.Get, request ,requestHeaderOptions ,cancellationToken);
        }


        public async Task<ProductReview> GetAsync(string productId ,string reveiwId,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format(BASE_URL_WITH_ID, productId, reveiwId);

            return await MakeRequestAsync<ProductReview>(path, HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }
    }
}
