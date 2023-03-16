using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class CategoryService
    {
        const string BASE_URL = "/catalog/categories";

        const string BASE_URL_WITH_ID = "/catalog/cateogries/{0}";

        private readonly MicroStoreClinet _microStoreClinet;

        public CategoryService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<Category> CreateAsync(CategoryRequestOptions options  ,  CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Category>(BASE_URL, HttpMethod.Post, options,  cancellationToken );
        }

        public async Task<List<Category>> ListAsync(SortingRequestOptions options , CancellationToken cancellationToken =default )
        {
            return await _microStoreClinet.MakeRequest<List<Category>>(BASE_URL, HttpMethod.Get, options, cancellationToken);
        }

        public async Task<Category> GetAsync(string categoryId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Category>(string.Format(BASE_URL_WITH_ID, categoryId),HttpMethod.Get ,cancellationToken: cancellationToken);
        }

        public async Task<Category> UpdateAsync(string categoryId , CategoryRequestOptions options ,CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Category>(string.Format(BASE_URL_WITH_ID, categoryId), HttpMethod.Put, options, cancellationToken);
        }
    }
}
