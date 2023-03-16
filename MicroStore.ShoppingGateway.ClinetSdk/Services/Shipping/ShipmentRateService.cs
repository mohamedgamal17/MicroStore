using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentRateService
    {
        const string BaseUrl = "/shipping/rates";

        private readonly MicroStoreClinet _microStoreClinet;

        public ShipmentRateService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


     

        public async Task<List<EstimatedRate>> EstimateAsync(ShipmentRateEstimateRequestOptions options , CancellationToken cancellationToken = default )
        {
            return await _microStoreClinet.MakeRequest<List<EstimatedRate>>(BaseUrl + "/" + "estimate", HttpMethod.Post, options,cancellationToken);
        }
    }
}
