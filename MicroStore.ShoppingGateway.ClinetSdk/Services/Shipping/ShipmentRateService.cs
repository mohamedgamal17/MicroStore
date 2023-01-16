using MicroStore.ShoppingGateway.ClinetSdk.Entities;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentRateService
    {
        const string BaseUrl = "/rates";

        private readonly MicroStoreClinet _microStoreClinet;

        public ShipmentRateService(MicroStoreClinet microStoreClinet)
        {
            _microStoreClinet = microStoreClinet;
        }


        public Task<HttpResponseResult<ListResult<ShipmentRate>>> RetrieveAsync(ShipmentRateRetrieveRequestOptions options , CancellationToken
             cancellationToken)
        {
            return _microStoreClinet.MakeRequest<ShipmentRateRetrieveRequestOptions, ListResult<ShipmentRate>>(options, BaseUrl +"/"+"retrive", HttpMethod.Post, cancellationToken);
        }

        public Task<HttpResponseResult<ListResult<EstimatedRate>>> EstimateAsync(ShipmentRateEstimateRequestOptions options , CancellationToken cancellationToken)
        {
            return _microStoreClinet.MakeRequest<ShipmentRateEstimateRequestOptions, ListResult<EstimatedRate>>(options, BaseUrl + "/" + "retrive", HttpMethod.Post, cancellationToken);
        }
    }
}
