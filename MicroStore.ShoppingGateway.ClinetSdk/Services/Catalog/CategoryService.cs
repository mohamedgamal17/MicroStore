using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
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

        public async Task<HttpResponseResult<Category>> CreateAsync(CategoryCreateOptions options  ,  CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Category>(BaseUrl, HttpMethod.Post, options,  cancellationToken );
        }

        public Task<HttpResponseResult<ListResult<CategoryList>>> ListAsync(SortingRequestOptions options , CancellationToken cancellationToken =default )
        {
            return _microStoreClinet.MakeRequest<ListResult <CategoryList>>(BaseUrl, HttpMethod.Get, options, cancellationToken);
        }

        public Task<HttpResponseResult<Category>> RetriveAsync(Guid categoryId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<Category>(string.Format("{0}/{1}",BaseUrl,categoryId),HttpMethod.Get ,cancellationToken: cancellationToken);
        }

        public Task<HttpResponseResult<Category>> UpdateAsync(Guid categoryId , CategoryUpdateOptions options ,CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<Category>(string.Format("{0}/{1}", BaseUrl, categoryId), HttpMethod.Put, options, cancellationToken);
        }
    }
}
