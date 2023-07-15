using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ManufacturerService
    {
        const string BASE_URL = "/catalog/manufacturers";

        const string BASE_URL_WITH_ID = "/catalog/manufacturers/{0}";

        private readonly MicroStoreClinet _microStoreClient;

        public ManufacturerService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }

        public async Task<List<Manufacturer>> ListAsync(ManufacturerListRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<List<Manufacturer>>(BASE_URL, HttpMethod.Get, options, cancellationToken);
        }

        public async Task<Manufacturer> GetAsync(string id , CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<Manufacturer>(string.Format(BASE_URL_WITH_ID, id),HttpMethod.Get ,cancellationToken);
        }

        public async Task<Manufacturer> CreateAsync(ManufacturerRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<Manufacturer>(BASE_URL, HttpMethod.Post, options, cancellationToken);
        }

        public async Task<Manufacturer> UpdateAsync(string id ,ManufacturerRequestOptions options,  CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<Manufacturer>(string.Format(BASE_URL_WITH_ID, id), HttpMethod.Put, options, cancellationToken);
        }
    }
}
