using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ManufacturerService : Service ,
        IListable<Manufacturer, ManufacturerListRequestOptions>,
        IRetrievable<Manufacturer>,
        ICreatable<Manufacturer, ManufacturerRequestOptions>,
        IUpdateable<Manufacturer, ManufacturerRequestOptions>
    {
        const string BASE_URL = "/catalog/manufacturers";

        const string BASE_URL_WITH_ID = "/catalog/manufacturers/{0}";

        public ManufacturerService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<List<Manufacturer>> ListAsync(ManufacturerListRequestOptions options, RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<List<Manufacturer>>(BASE_URL, HttpMethod.Get, options , requestHeaderOptions, cancellationToken);
        }

        public async Task<Manufacturer> GetAsync(string id, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Manufacturer>(string.Format(BASE_URL_WITH_ID, id),httpMethod: HttpMethod.Get, requestHeaderOptions: requestHeaderOptions ,cancellationToken: cancellationToken);
        }

        public async Task<Manufacturer> CreateAsync(ManufacturerRequestOptions options , RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Manufacturer>(BASE_URL, HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }

        public async Task<Manufacturer> UpdateAsync(string id ,ManufacturerRequestOptions options, RequestHeaderOptions requestHeaderOptions = null , CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Manufacturer>(string.Format(BASE_URL_WITH_ID, id), HttpMethod.Put, options,  requestHeaderOptions,cancellationToken);
        }
    }
}
