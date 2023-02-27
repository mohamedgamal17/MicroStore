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


        public async Task<Product> CreateAsync(string productId , ProductCategoryCreateOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>(string.Format(BaseUrl, productId), HttpMethod.Post, options, cancellationToken);
        }

        public async Task<Product> UpdateAsync(string productId , string categoryId , ProductCategoryUpdateOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>(string.Format(BaseUrl, productId) + "/" + categoryId, HttpMethod.Put, options, cancellationToken);
        }

        public async Task<Product> DeleteAsync(string productId, string categoryId,  CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest< Product>(string.Format(BaseUrl, productId) + "/" + categoryId, HttpMethod.Delete, cancellationToken);
        }
    }
}
