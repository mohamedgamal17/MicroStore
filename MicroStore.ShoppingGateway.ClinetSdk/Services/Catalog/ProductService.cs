using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductService
    {

        private readonly MicroStoreClinet _clinet;

        const string BaseUrl = "/products";
        public ProductService(MicroStoreClinet clinet)
        {
            _clinet = clinet;
        }

        public Task<HttpResponseResult<Product>> CreateAsync(ProductCreateOptions options, CancellationToken cancellationToken)
        {
            return _clinet.MakeRequest<ProductCreateOptions, Product>(options, BaseUrl, HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult<PagedList<ProductList>>> ListAsync(PagingAndSortingRequestOptions options, CancellationToken cancellationToken)
        {
            return _clinet.MakeRequest<PagingAndSortingRequestOptions,PagedList<ProductList>>(options,BaseUrl,HttpMethod.Get, cancellationToken);
        }

        public Task<HttpResponseResult<Product>> RetriveAsync(Guid productId , CancellationToken cancellationToken)
        {
            return _clinet.MakeRequest<EmptyRequst, Product>(EmptyRequst.Empty, string.Format("{0}/{1}", BaseUrl, productId), HttpMethod.Get, cancellationToken);
        }
                  
        public Task<HttpResponseResult<Product>> UpdateAsync(Guid productId,  ProductUpdateOptions options, CancellationToken cancellationToken)
        {
            return _clinet.MakeRequest<ProductUpdateOptions, Product>(options, string.Format("{0}/{1}", BaseUrl, productId), HttpMethod.Put, cancellationToken);
        }
    }
}
