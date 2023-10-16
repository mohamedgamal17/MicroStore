using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductService
    {

        private readonly MicroStoreClinet _clinet;

        const string BASE_URL_WITHOUT_ID = "/catalog/products";

        const string BASE_URL_WITH_ID = "/catalog/products/{0}";

        const string PRODUCT_IMAGE_URL_WITHOUT_ID = "/catalog/products/{0}/productimages";

        const string PRODUCT_IMAGE_URL_WITH_ID = "/catalog/products/{0}/productimages/{1}";
        public ProductService(MicroStoreClinet clinet)
        {
            _clinet = clinet;
        }

        public async Task<Product> CreateAsync(ProductRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _clinet.MakeRequest<Product>( BASE_URL_WITHOUT_ID, HttpMethod.Post ,options, cancellationToken);
        }

        public Task<PagedList<Product>> ListAsync(ProductListRequestOptions options, CancellationToken cancellationToken = default)
        {
            return _clinet.MakeRequest<PagedList<Product>>(BASE_URL_WITHOUT_ID,HttpMethod.Get, options, cancellationToken);
        }

        public async Task<Product> GetAsync(string productId , CancellationToken cancellationToken = default)
        {
            return await _clinet.MakeRequest<Product>(string.Format(BASE_URL_WITH_ID ,  productId), HttpMethod.Get, cancellationToken);
        }
                  
        public async Task<Product> UpdateAsync(string productId,  ProductRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _clinet.MakeRequest<Product>(string.Format(BASE_URL_WITH_ID , productId), HttpMethod.Put, options, cancellationToken);
        }

        public async Task<List<ProductImage>> ListProductImageAsync(string productId , CancellationToken cancellationToken = default)
        {
            return await _clinet.MakeRequest<List<ProductImage>>(string.Format(PRODUCT_IMAGE_URL_WITHOUT_ID, productId), HttpMethod.Get, cancellationToken);
        }

        public async Task<Product> CreateProductImageAsync(string productId, ProductImageRequestCreateOptions options, CancellationToken cancellationToken = default)
        {
            return await _clinet.MakeRequest<Product>(string.Format(PRODUCT_IMAGE_URL_WITHOUT_ID, productId), HttpMethod.Post, options, cancellationToken);
        }

        public async Task<Product> UpdateProductImageAsync(string productId, string productImageId, ProductImageRequestUpdateOptions options , CancellationToken  cancellationToken = default)
        {
            return await _clinet.MakeRequest<Product>(string.Format(PRODUCT_IMAGE_URL_WITH_ID,productId,productImageId),HttpMethod.Put, options, cancellationToken);
        }

        public async Task<Product> DeleteProductImageAsync(string productId , string  productImageId, CancellationToken cancellationToken = default)
        {
            return await _clinet.MakeRequest<Product>(string.Format(PRODUCT_IMAGE_URL_WITH_ID, productId, productImageId), HttpMethod.Delete, cancellationToken);
        }


        public async Task<List<Product>> SearchByImage(ProductSearchByImageRequestOptions options, CancellationToken cancellationToken = default)
        {
            string path = string.Format("{0}/{1}", BASE_URL_WITHOUT_ID, "search-by-image");

            return await _clinet.MakeRequest<List<Product>>(path, HttpMethod.Post, options, cancellationToken);
        }
     }
}
