using MicroStore.ShoppingGateway.ClinetSdk.Entities.Geographic;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Geographic
{
    public class CountryService
    {
        const string BASE_URL = "/geographic/countries";

        private readonly MicroStoreClinet _microStoreClinet;

        public CountryService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<Country> CreateAsync(CountryRequestOptions request, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Country>(BASE_URL, HttpMethod.Post, request, cancellationToken);
        }

        public async Task<Country> UpdateAsync(string id, CountryRequestOptions request ,  CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Country>(string.Format("{0}/{1}",BASE_URL,id), HttpMethod.Put, request, cancellationToken);
        }


        public async Task DeleteAsync(string id , CancellationToken cancellationToken = default)
        {
            await _microStoreClinet.MakeRequest(string.Format("{0}/{1}", BASE_URL, id), HttpMethod.Delete, cancellationToken);
        }


        public async Task<List<Country>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<List<Country>>(BASE_URL,HttpMethod.Get, cancellationToken);
        }

        public async Task<Country> GetAsync(string id , bool includeStateProvince = true , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Country>(string.Format("{0}/{1}", BASE_URL, id),  HttpMethod.Get, new { IncludeStateProvince = includeStateProvince }, cancellationToken);
        }

        public async Task<Country> GetByCodeAsync(string code , bool includeStateProvince = true, CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Country>(string.Format("{0}/code/{1}", BASE_URL, code), HttpMethod.Get, new { IncludeStateProvince = includeStateProvince }, cancellationToken);
        }
    }
}
