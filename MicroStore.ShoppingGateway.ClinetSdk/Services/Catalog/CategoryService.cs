using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Extensions;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class CategoryService
    {
        const string BaseUrl = "/categories";

        private readonly MicroStoreClinet _microStoreClinet;

        public CategoryService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public Task<HttpResponseResult<Category>> CreateAsync(CategoryCreateOptions options  ,  CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<CategoryCreateOptions, Category>(options, BaseUrl, HttpMethod.Post, cancellationToken = default);
        }

        public Task<HttpResponseResult<ListResult<CategoryList>>> ListAsync(SortingRequestOptions options , CancellationToken cancellationToken =default )
        {
            return _microStoreClinet.MakeGetRequest<ListResult <CategoryList>>(BaseUrl, options.ConvertToDictionary(), cancellationToken);
        }

        public Task<HttpResponseResult<Category>> RetriveAsync(Guid categoryId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeGetRequest<Category>(uri : string.Format("{0}/{1}",BaseUrl,categoryId), cancellationToken: cancellationToken);
        }

        public Task<HttpResponseResult<Category>> UpdateAsync(Guid categoryId , CategoryUpdateOptions options ,CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<CategoryUpdateOptions, Category>(options, string.Format("{0}/{1}", BaseUrl, categoryId), HttpMethod.Put, cancellationToken);
        }
    }
}
