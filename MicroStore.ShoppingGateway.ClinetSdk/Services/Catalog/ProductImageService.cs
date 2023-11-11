using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductImageService : Service ,
        INestedListable<ProductImage>,
        INestedRetrievable<ProductImage>,
        INestedCreatable<ProductImage,ProductImageRequestCreateOptions>,
        INestedUpdateable<ProductImage, ProductImageRequestUpdateOptions>,
        INestedDeletable
    {
        const string BASE_URL_WITHOUT_ID = "/catalog/products/{0}/productimages";

        const string BASE_URL_WITH_ID = "/catalog/products/{0}/productimages/{1}";

        public ProductImageService(MicroStoreClinet microStoreClinet)
            : base(microStoreClinet)
        {
        }

        public async Task<List<ProductImage>> ListAsync(string parentId, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<List<ProductImage>>(string.Format(BASE_URL_WITHOUT_ID, parentId), httpMethod : HttpMethod.Get, requestHeaderOptions: requestHeaderOptions,cancellationToken: cancellationToken);
        }

        public async Task<ProductImage> GetAsync(string parentId, string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<ProductImage>(string.Format(BASE_URL_WITH_ID, parentId,id), httpMethod: HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }

        public async Task<ProductImage> CreateAsync(string parentId, ProductImageRequestCreateOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<ProductImage>(string.Format(BASE_URL_WITHOUT_ID, parentId), HttpMethod.Post, options,requestHeaderOptions, cancellationToken);
        }

        public async Task<ProductImage> UpdateAsync(string parentId, string id, ProductImageRequestUpdateOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<ProductImage>(string.Format(BASE_URL_WITHOUT_ID, parentId, id), HttpMethod.Put, options,requestHeaderOptions ,cancellationToken);
        }


        public async Task DeleteAsync(string parentId, string id, RequestHeaderOptions options = null, CancellationToken cancellationToken = default)
        {
             await MakeRequestAsync(string.Format(BASE_URL_WITH_ID, parentId, id),httpMethod: HttpMethod.Delete, cancellationToken : cancellationToken);
        }
    }
}
