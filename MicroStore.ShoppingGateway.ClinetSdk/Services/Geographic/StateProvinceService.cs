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


        public async Task<StateProvinceService> CreateAsync(string countryId ,StateProvinceRequestOptions request, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvinceService>(string.Format(BASE_URL, countryId), HttpMethod.Post, request, cancellationToken);
        }

        public async Task<StateProvinceService> UpdateAsync(string countryId , string stateId , StateProvinceRequestOptions request, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvinceService>(string.Format(BASE_URL,countryId) + "/" + stateId, HttpMethod.Put, request, cancellationToken);
        }
        public async Task<StateProvinceService> DeleteAsync(string countryId, string stateId , CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvinceService>(string.Format(BASE_URL, countryId) + "/" + stateId, HttpMethod.Delete,  cancellationToken);
        }

        public async Task<List<StateProvinceService>> ListAsync(string countryId, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<List<StateProvinceService>>(string.Format(BASE_URL, countryId), HttpMethod.Get,  cancellationToken);
        }

        public async Task<StateProvinceService> GetAsync(string countryId, string stateId, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvinceService>(string.Format(BASE_URL, countryId) + "/" + stateId, HttpMethod.Get, cancellationToken);
        }

        public async Task<StateProvince> GetByCodeAsync(string countryCode, string stateCode, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<StateProvince>(string.Format(BASE_URL, $"code/{ countryCode}") + "code/" + stateCode, HttpMethod.Get, cancellationToken);
        }
    }
}
