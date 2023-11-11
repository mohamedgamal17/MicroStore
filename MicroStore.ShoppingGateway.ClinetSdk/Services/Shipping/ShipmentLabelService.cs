using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentLabelService : Service
    {

        const string BaseUrl = "/shipping/labels";

        public ShipmentLabelService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<Shipment> BuyLabelAsync(PurchaseLabelRequestOptions options , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<Shipment>( BaseUrl + "/" + "buylabel", HttpMethod.Post, options,requestHeaderOptions ,cancellationToken);
        }
    }
}
