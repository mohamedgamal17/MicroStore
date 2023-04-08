using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic
{
    public class StateProvinceService
    {
        const string BASE_URL = "/geographic/countries/{0}/states";


        private readonly MicroStoreClinet _microStoreClient;

        public StateProvinceService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }


        public async Task<StateProvince> CreateAsync(string countryId ,StateProvinceRequestOptions request, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvince>(string.Format(BASE_URL, countryId), HttpMethod.Post, request, cancellationToken);
        }

        public async Task<StateProvince> UpdateAsync(string countryId , string stateId , StateProvinceRequestOptions request, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvince>(string.Format(BASE_URL,countryId) + "/" + stateId, HttpMethod.Put, request, cancellationToken);
        }
        public async Task DeleteAsync(string countryId, string stateId , CancellationToken cancellationToken = default)
        {
             await _microStoreClient.MakeRequest(string.Format(BASE_URL, countryId) + "/" + stateId, HttpMethod.Delete,  cancellationToken);
        }

        public async Task<List<StateProvince>> ListAsync(string countryId, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<List<StateProvince>>(string.Format(BASE_URL, countryId), HttpMethod.Get,  cancellationToken);
        }

        public async Task<StateProvince> GetAsync(string countryId, string stateId, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvince>(string.Format(BASE_URL, countryId) + "/" + stateId, HttpMethod.Get, cancellationToken);
        }

        public async Task<StateProvince> GetByCodeAsync(string countryCode, string stateCode, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvince>(string.Format(BASE_URL, $"code/{ countryCode}") + "/code/" + stateCode, HttpMethod.Get, cancellationToken);
        }
    }
}
