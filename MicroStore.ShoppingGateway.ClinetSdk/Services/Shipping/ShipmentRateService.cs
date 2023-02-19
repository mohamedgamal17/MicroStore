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


        public async Task<HttpResponseResult<ListResult<ShipmentRate>>> RetrieveAsync(ShipmentRateRetrieveRequestOptions options , CancellationToken
             cancellationToken = default)
        {
            return await _microStoreClinet.MakeRequest<ListResult<ShipmentRate>>(BaseUrl + "/" + "retrive", HttpMethod.Post,options, cancellationToken);
        }

        public async Task<HttpResponseResult<ListResult<EstimatedRate>>> EstimateAsync(ShipmentRateEstimateRequestOptions options , CancellationToken cancellationToken = default )
        {
            return await _microStoreClinet.MakeRequest<ListResult<EstimatedRate>>(BaseUrl + "/" + "estimate", HttpMethod.Post, options,cancellationToken);
        }
    }
}
