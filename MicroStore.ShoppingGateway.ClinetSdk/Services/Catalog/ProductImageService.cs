using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductImageService
    {
        const string BaseUrl = "/catalog/products/{0}/productimages";

        private readonly MicroStoreClinet _microStoreClinet;

        public ProductImageService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


        public async Task<Product> CreateAsync(string productId , ProductImageCreateOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>(string.Format(BaseUrl, productId), HttpMethod.Post, options, cancellationToken);
        }

        public async Task<Product> UpdateAsync(string productId , string productImageId  , ProductImageUpdateOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>( string.Format(BaseUrl, productId) + "/" + productImageId, HttpMethod.Put , options, cancellationToken);
        }

        public async Task<Product> DeleteAsync(string productId , string productImageId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>(string.Format(BaseUrl, productId) + "/" + productImageId, HttpMethod.Delete, cancellationToken);
        }

    }
}
