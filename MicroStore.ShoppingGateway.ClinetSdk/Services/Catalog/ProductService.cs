using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductService : Service,
        ICreatable<Product, ProductRequestOptions>,
        IUpdateable<Product, ProductRequestOptions>,
        IListableWithPaging<Product,ProductListRequestOptions>,
        IRetrievable<Product>
    {
        const string BASE_URL_WITHOUT_ID = "/catalog/products";

        const string BASE_URL_WITH_ID = "/catalog/products/{0}";

        public ProductService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<Product> CreateAsync(ProductRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Product>( BASE_URL_WITHOUT_ID, HttpMethod.Post ,options, requestHeaderOptions, cancellationToken);
        }

        public async Task<PagedList<Product>> ListAsync(ProductListRequestOptions options,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<PagedList<Product>>(BASE_URL_WITHOUT_ID,HttpMethod.Get, options,requestHeaderOptions ,cancellationToken);
        }

        public async Task<Product> GetAsync(string productId , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Product>(string.Format(BASE_URL_WITH_ID ,  productId), HttpMethod.Get,requestHeaderOptions: requestHeaderOptions,cancellationToken: cancellationToken);
        }
                  
        public async Task<Product> UpdateAsync(string productId,  ProductRequestOptions options, RequestHeaderOptions requestHeaderOptions = null,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Product>(string.Format(BASE_URL_WITH_ID , productId), HttpMethod.Put, options, requestHeaderOptions, cancellationToken);
        }
        public async Task<PagedList<Product>> SearchByImage(ProductSearchByImageRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BASE_URL_WITHOUT_ID, "search-by-image");

            return await MakeRequestAsync<PagedList<Product>>(path, HttpMethod.Get, options, requestHeaderOptions, cancellationToken);
        }
     }
}
