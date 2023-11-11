using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentSettingsService : Service
    {
        const string BASE_URL = "/shipping/settings";

        public ShipmentSettingsService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<ShipmentSettings> GetAsync(RequestHeaderOptions requestHeaderOptions = null,CancellationToken cancellationToken = default)
        {
           return await MakeRequestAsync<ShipmentSettings>(BASE_URL, HttpMethod.Get, cancellationToken: cancellationToken);
        }
        public async Task<ShipmentSettings> UpdateAsync(ShipmentSettingsRequestOptions options = null, RequestHeaderOptions requestHeaderOptions = null, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<ShipmentSettings>(BASE_URL, HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }
    }
}
