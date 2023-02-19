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


        public async Task<HttpResponseResult<Product>> CreateAsync(Guid productId , ProductImageCreateOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>(string.Format(BaseUrl, productId), HttpMethod.Post, options, cancellationToken);
        }

        public async Task<HttpResponseResult<Product>> UpdateAsync(Guid productId , Guid productImageId  , ProductImageUpdateOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>( string.Format(BaseUrl, productId) + "/" + productImageId, HttpMethod.Put , options, cancellationToken);
        }

        public async Task<HttpResponseResult<Product>> DeleteAsync(Guid productId , Guid productImageId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Product>(string.Format(BaseUrl, productId) + "/" + productImageId, HttpMethod.Delete, cancellationToken);
        }

    }
}
