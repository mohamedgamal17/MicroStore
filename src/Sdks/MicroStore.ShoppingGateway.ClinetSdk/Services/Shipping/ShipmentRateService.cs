using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentRateService : Service
    {
        const string BaseUrl = "/shipping/rates";

        public ShipmentRateService(MicroStoreClinet microStoreClinet) : base(microStoreClinet)
        {
        }
        public async Task<List<EstimatedRate>> EstimateAsync(ShipmentRateEstimateRequestOptions options , RequestHeaderOptions requestHeaderOptions = null ,CancellationToken cancellationToken = default )
        {
            return await MakeRequestAsync<List<EstimatedRate>>(BaseUrl + "/" + "estimate", HttpMethod.Post, options, requestHeaderOptions, cancellationToken);
        }
    }
}
