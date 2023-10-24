using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShippingSystemService
    {
        const string BASE_URL = "/shipping/systems";

        private readonly MicroStoreClinet _microStoreClient;

        public ShippingSystemService(MicroStoreClinet microStoreClient)
        {
            _microStoreClient = microStoreClient;
        }


        public async Task<List<ShipmentSystem>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _microStoreClient.MakeRequest<List<ShipmentSystem>>(BASE_URL, HttpMethod.Get, cancellationToken: cancellationToken);
        }
    }
}
