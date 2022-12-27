using Microsoft.Extensions.Options;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Domain;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Settings;
using Newtonsoft.Json;
using ShipEngineSDK;
using ShipEngineSDK.GetRatesWithShipmentDetails;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Shipping.Plugin.ShipEngineGateway
{
    public class ShipEngineClinet : ShipEngine , IShipEngineClinet , ITransientDependency
    {
        private readonly ShipEngineSettings _settings;

        public ShipEngineClinet(IOptions<ShipEngineSettings> options) : base(options.Value.ApiKey)
        {
            _settings = options.Value;
        }

        public async Task<ShipmentRateResult> EstimateRate(EstimateRate estimateRate)
        {
            string path = "/v1/rates/estimate";

            var json = JsonConvert.SerializeObject(estimateRate, JsonSerializerSettings);

            return await SendHttpRequestAsync<ShipmentRateResult>(HttpMethod.Post, path, json, _client, _config);
        }

        public async Task<ShipEngineShipment> CreateShipment(Shipment shipment)
        {

            string path = $"v1/shipments";

            var json = JsonConvert.SerializeObject(shipment, JsonSerializerSettings);

            return await SendHttpRequestAsync<ShipEngineShipment>(HttpMethod.Post, path, json, _client, _config);
        }

        public  async Task<ShipmentRateResult> EstimateShipment(string shipmentId)
        {
            string path = $"v1/shipments/{shipmentId}/rates";

            return await SendHttpRequestAsync<ShipmentRateResult>(HttpMethod.Get,path,string.Empty, _client, _config);
        }

    }


 
}
