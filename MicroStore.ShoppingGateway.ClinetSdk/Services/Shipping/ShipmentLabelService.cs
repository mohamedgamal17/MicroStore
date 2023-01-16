using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentLabelService
    {

        const string BaseUrl = "/labels";

        private readonly MicroStoreClinet _microStoreClinet;

        public ShipmentLabelService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }

        public Task<HttpResponseResult<Shipment>> BuyLabelAsync(ShipmentLabelBuyRequestOptions options , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeRequest<ShipmentLabelBuyRequestOptions, Shipment>(options, BaseUrl + "/" + "buylabel", HttpMethod.Post, cancellationToken);
        }
    }
}
