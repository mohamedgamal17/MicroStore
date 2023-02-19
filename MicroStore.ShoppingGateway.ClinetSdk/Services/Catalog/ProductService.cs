using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductService
    {

        private readonly MicroStoreClinet _clinet;

        const string BaseUrl = "/catalog/products";
        public ProductService(MicroStoreClinet clinet)
        {
            _clinet = clinet;
        }

        public async Task<HttpResponseResult<Product>> CreateAsync(ProductCreateOptions options, CancellationToken cancellationToken)
        {
            return await _clinet.MakeRequest<Product>( BaseUrl, HttpMethod.Post ,options, cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<ProductList>>> ListAsync(PagingAndSortingRequestOptions options, CancellationToken cancellationToken = default)
        {
            return _clinet.MakeRequest<PagedList<ProductList>>(BaseUrl,HttpMethod.Get, options, cancellationToken);
        }

        public async Task<HttpResponseResult<Product>> RetriveAsync(Guid productId , CancellationToken cancellationToken = default)
        {
            return await _clinet.MakeRequest<Product>(string.Format("{0}/{1}", BaseUrl, productId), HttpMethod.Get, cancellationToken);
        }
                  
        public async Task<HttpResponseResult<Product>> UpdateAsync(Guid productId,  ProductUpdateOptions options, CancellationToken cancellationToken = default)
        {
            return await _clinet.MakeRequest<Product>(string.Format("{0}/{1}", BaseUrl, productId), HttpMethod.Put, options, cancellationToken);
        }
    }
}
