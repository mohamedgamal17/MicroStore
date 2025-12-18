using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic
{
    public class CountryService : Service, 
        IListable<Country,CountryListRequestOptions>,
        IRetrievable<Country>,
        ICreatable<Country,CountryRequestOptions>,
        IUpdateable<Country,CountryRequestOptions>,
        IDeletable
    {
        const string BASE_URL = "/geographic/countries";

        public CountryService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<Country> CreateAsync(CountryRequestOptions request, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Country>(BASE_URL, HttpMethod.Post, request, requestHeaderOptions, cancellationToken);
        }

        public async Task<Country> UpdateAsync(string id, CountryRequestOptions request , RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Country>(string.Format("{0}/{1}",BASE_URL,id), HttpMethod.Put, request, requestHeaderOptions, cancellationToken);
        }

        public async Task DeleteAsync(string id ,RequestHeaderOptions requestHeaderOptions ,CancellationToken cancellationToken = default)
        {
            await MakeRequestAsync(string.Format("{0}/{1}", BASE_URL, id), HttpMethod.Delete, requestHeaderOptions: requestHeaderOptions,cancellationToken: cancellationToken);
        }

        public async Task<List<Country>> ListAsync(CountryListRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null , CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<List<Country>>(BASE_URL,HttpMethod.Get,options,requestHeaderOptions ,cancellationToken);
        }

        public async Task<Country> GetAsync(string id ,RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Country>(string.Format("{0}/{1}", BASE_URL, id),  HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }

        public async Task<Country> GetByCodeAsync(string code ,  RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Country>(string.Format("{0}/code/{1}", BASE_URL, code), HttpMethod.Get, requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }
    }
}
