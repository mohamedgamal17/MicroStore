using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductCategoryService
    {

        const string BaseUrl = "products/{0}/productcateogries";

        private readonly MicroStoreClinet _microStoreClinet;

        public ProductCategoryService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


        public Task<HttpResponseResult<Product>> CreateAsync(Guid productId , ProductCategoryCreateOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<ProductCategoryCreateOptions, Product>(options, string.Format(BaseUrl, productId), HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult<Product>> UpdateAsync(Guid productId , Guid categoryId , ProductCategoryUpdateOptions options , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<ProductCategoryUpdateOptions, Product>(options, string.Format(BaseUrl, productId) + "/" + categoryId, HttpMethod.Put, cancellationToken);
        }

        public Task<HttpResponseResult<Product>> DeleteAsync(Guid productId, Guid categoryId, ProductCategoryUpdateOptions options, CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<EmptyRequst, Product>(EmptyRequst.Empty, string.Format(BaseUrl, productId) + "/" + categoryId, HttpMethod.Delete, cancellationToken);
        }
    }
}
