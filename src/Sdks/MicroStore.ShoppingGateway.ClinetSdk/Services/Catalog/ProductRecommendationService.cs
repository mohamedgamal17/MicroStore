using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductRecommendationService : Service
    {

        const string BASE_URL = "/catalog/product-recommandation";

        public ProductRecommendationService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<PagedList<Product>> GetProductRecommendation(PagingReqeustOptions request,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = BASE_URL + "/" + "user-recommandation";

            return await MakeRequestAsync<PagedList<Product>>(path,HttpMethod.Get, request,requestHeaderOptions ,cancellationToken: cancellationToken);
        }


        public async Task<PagedList<Product>> GetSimilarItems(string productId , PagingReqeustOptions request, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BASE_URL, "similar-items", productId);

            return await MakeRequestAsync<PagedList<Product>>(path,HttpMethod.Get,request, requestHeaderOptions, cancellationToken);
        }
    }
}
