using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentSettingsService
    {
        const string BASE_URL = "/shipping/settings";

        private readonly MicroStoreClinet _microStoreClient;

        public ShipmentSettingsService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }
        public async Task<ShipmentSettings> RetriveShipmentSettingsAsync(CancellationToken cancellationToken = default)
        {
           return await _microStoreClient.MakeRequest<ShipmentSettings>(BASE_URL, HttpMethod.Get, cancellationToken: cancellationToken);
        }
        public async Task<ShipmentSettings> UpdateShipmentSettingsAsync(ShipmentSettingsRequestOptions options, CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<ShipmentSettings>(BASE_URL, HttpMethod.Post, options, cancellationToken);
        }
    }
}
