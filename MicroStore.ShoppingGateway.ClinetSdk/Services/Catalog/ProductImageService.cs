using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductImageService
    {
        const string BaseUrl = "/products/{0}/productimages";

        private readonly MicroStoreClinet _microStoreClinet;

        public ProductImageService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


        public Task<HttpResponseResult<Product>> CreateAsync(Guid productId , ProductImageCreateOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<ProductImageCreateOptions, Product>(options, string.Format(BaseUrl, productId), HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult<Product>> UpdateAsync(Guid productId , Guid productImageId  , ProductImageUpdateOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<ProductImageUpdateOptions, Product>(options, string.Format(BaseUrl, productId) + "/" + productImageId, HttpMethod.Put, cancellationToken);
        }

        public Task<HttpResponseResult<Product>> DeleteAsync(Guid productId , Guid productImageId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<EmptyRequst, Product>(EmptyRequst.Empty, string.Format(BaseUrl, productId) + "/" + productImageId, HttpMethod.Delete, cancellationToken);
        }

    }
}
