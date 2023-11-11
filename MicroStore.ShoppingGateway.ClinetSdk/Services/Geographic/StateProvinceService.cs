using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic
{
    public class StateProvinceService  : Service,
        INestedListable<StateProvince>,
        INestedRetrievable<StateProvince>,
        INestedCreatable<StateProvince, StateProvinceRequestOptions>,
        INestedUpdateable<StateProvince,StateProvinceRequestOptions>,
        INestedDeletable
    {
        const string BASE_URL = "/geographic/countries/{0}/states";

        public StateProvinceService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<List<StateProvince>> ListAsync(string parentId, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<List<StateProvince>>(string.Format(BASE_URL, parentId), HttpMethod.Get,requestHeaderOptions: requestHeaderOptions, cancellationToken: cancellationToken);
        }
        public async Task<StateProvince> CreateAsync(string parentId ,StateProvinceRequestOptions request, RequestHeaderOptions requestHeaderOption = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<StateProvince>(string.Format(BASE_URL, parentId), HttpMethod.Post, request,requestHeaderOption ,cancellationToken);
        }

        public async Task<StateProvince> UpdateAsync(string parentId, string id , StateProvinceRequestOptions request, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<StateProvince>(string.Format(BASE_URL, parentId) + "/" + id, HttpMethod.Put, request, requestHeaderOptions, cancellationToken);
        }
        public async Task DeleteAsync(string parentId, string id ,RequestHeaderOptions requestHeaderOptions = null , CancellationToken cancellationToken = default)
        {
             await MakeRequestAsync(string.Format(BASE_URL, parentId) + "/" + id, HttpMethod.Delete, requestHeaderOptions: requestHeaderOptions ,cancellationToken: cancellationToken);
        }
        public async Task<StateProvince> GetAsync(string parentId, string id, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<StateProvince>(string.Format(BASE_URL, parentId) + "/" + id, HttpMethod.Get, requestHeaderOptions: requestHeaderOptions ,cancellationToken: cancellationToken);
        }

        public async Task<StateProvince> GetByCodeAsync(string countryCode, string stateCode, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<StateProvince>(string.Format(BASE_URL, $"code/{countryCode}") + "/code/" + stateCode, HttpMethod.Get,requestHeaderOptions: requestHeaderOptions,cancellationToken: cancellationToken);
        }
    }
}
