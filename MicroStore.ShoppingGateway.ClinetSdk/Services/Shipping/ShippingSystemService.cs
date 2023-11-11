using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using MicroStore.ShoppingGateway.ClinetSdk.Interfaces;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShippingSystemService : Service,
        IListable<ShipmentSystem>
    {
        const string BASE_URL = "/shipping/systems";

        public ShippingSystemService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }

        public async Task<List<ShipmentSystem>> ListAsync(RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<List<ShipmentSystem>>(BASE_URL, HttpMethod.Get,requestHeaderOptions: requestHeaderOptions ,cancellationToken: cancellationToken);
        }
    }
}
