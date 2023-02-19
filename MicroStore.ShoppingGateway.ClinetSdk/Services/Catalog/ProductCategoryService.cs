using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductCategoryService
    {

        const string BaseUrl = "/catalog/products/{0}/productcateogries";

        private readonly MicroStoreClinet _microStoreClinet;

        public ProductCategoryService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


        public async Task<HttpResponseResult<Product>> CreateAsync(Guid productId , ProductCategoryCreateOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>(string.Format(BaseUrl, productId), HttpMethod.Post, options, cancellationToken);
        }

        public async Task<HttpResponseResult<Product>> UpdateAsync(Guid productId , Guid categoryId , ProductCategoryUpdateOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest< Product>(string.Format(BaseUrl, productId) + "/" + categoryId, HttpMethod.Put, options, cancellationToken);
        }

        public async Task<HttpResponseResult<Product>> DeleteAsync(Guid productId, Guid categoryId,  CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest< Product>(string.Format(BaseUrl, productId) + "/" + categoryId, HttpMethod.Delete, cancellationToken);
        }
    }
}
