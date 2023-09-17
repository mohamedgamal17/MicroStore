using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductRecommendationService
    {

        const string BASE_URL = "/catalog/product-recommandation";

        private readonly MicroStoreClinet _microStoreClient;

        public ProductRecommendationService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }

        public async Task<PagedList<Product>> GetProductRecommendation(PagingReqeustOptions request, CancellationToken cancellationToken = default)
        {
            string path = BASE_URL + "/" + "user-recommandation";

            return await _microStoreClient.MakeRequest<PagedList<Product>>(path,HttpMethod.Get, request, cancellationToken);
        }


        public async Task<PagedList<Product>> GetSimilarItems(string productId , PagingReqeustOptions request, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}/{2}", BASE_URL, "similar-items", productId);

            return await _microStoreClient.MakeRequest<PagedList<Product>>(path,HttpMethod.Get,request, cancellationToken);
        }
    }
}
