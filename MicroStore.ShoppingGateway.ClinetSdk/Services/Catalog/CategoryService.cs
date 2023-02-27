using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class CategoryService
    {
        const string BaseUrl = "/catalog/categories";

        private readonly MicroStoreClinet _microStoreClinet;

        public CategoryService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<Category> CreateAsync(CategoryCreateOptions options  ,  CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Category>(BaseUrl, HttpMethod.Post, options,  cancellationToken );
        }

        public Task<List<CategoryList>> ListAsync(SortingRequestOptions options , CancellationToken cancellationToken =default )
        {
            return _microStoreClinet.MakeRequest<List<CategoryList>>(BaseUrl, HttpMethod.Get, options, cancellationToken);
        }

        public Task<Category> RetriveAsync(string categoryId , CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<Category>(string.Format("{0}/{1}",BaseUrl,categoryId),HttpMethod.Get ,cancellationToken: cancellationToken);
        }

        public Task<Category> UpdateAsync(string categoryId , CategoryUpdateOptions options ,CancellationToken cancellationToken = default)
        {
            return _microStoreClinet.MakeRequest<Category>(string.Format("{0}/{1}", BaseUrl, categoryId), HttpMethod.Put, options, cancellationToken);
        }
    }
}
