using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentLabelService
    {

        const string BaseUrl = "/shipping/labels";

        private readonly MicroStoreClinet _microStoreClinet;

        public ShipmentLabelService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public async Task<Shipment> BuyLabelAsync(ShipmentLabelBuyRequestOptions options , CancellationToken cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<Shipment>( BaseUrl + "/" + "buylabel", HttpMethod.Post, options, cancellationToken);
        }
    }
}
