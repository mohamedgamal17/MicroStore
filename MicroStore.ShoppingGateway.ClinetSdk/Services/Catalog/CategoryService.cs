using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class CategoryService : Service , 
        IListable<Category, CategoryListRequestOptions>  ,
        IRetrievable<Category>,
        ICreatable<Category, CategoryRequestOptions>,
        IUpdateable<Category, CategoryRequestOptions>
    {
        const string BASE_URL = "/catalog/categories";

        const string BASE_URL_WITH_ID = "/catalog/categories/{0}";

        public CategoryService(MicroStoreClinet microStoreClinet) 
            : base(microStoreClinet)
        {
        }

        public async Task<Category> CreateAsync(CategoryRequestOptions options  , RequestHeaderOptions requestHeaderOptions = null,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Category>(BASE_URL, HttpMethod.Post, options, requestHeaderOptions ,cancellationToken );
        }

        public async Task<List<Category>> ListAsync(CategoryListRequestOptions options , RequestHeaderOptions requestHeaderOptions = null,  CancellationToken cancellationToken =default )
        {
            return await MakeRequestAsync<List<Category>>(BASE_URL, HttpMethod.Get, options,requestHeaderOptions ,cancellationToken);
        }

        public async Task<Category> GetAsync(string categoryId ,RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Category>(string.Format(BASE_URL_WITH_ID, categoryId), 
                httpMethod: HttpMethod.Get,requestHeaderOptions:requestHeaderOptions ,cancellationToken: cancellationToken);
        }

        public async Task<Category> UpdateAsync(string categoryId , CategoryRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Category>(string.Format(BASE_URL_WITH_ID, categoryId), HttpMethod.Put, options, requestHeaderOptions, cancellationToken);
        }
    }
}
