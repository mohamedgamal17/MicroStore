using AutoMapper.Internal.Mappers;
using MicroStore.Shipping.Application.Abstraction.Configuration;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Common;
using MicroStore.Shipping.Plugin.ShipEngineGateway.Domain;
using Newtonsoft.Json;
using ShipEngineSDK;
using ShipEngineSDK.Common;
namespace MicroStore.Shipping.Plugin.ShipEngineGateway
{
    public class ShipEngineClinet : ShipEngine , IShipEngineClinet 
    {
        private readonly ShipmentProviderSetting _settings;

        public ShipEngineClinet(ShipmentProviderSetting settings) 
            : base(new Config(settings.ApiKey, TimeSpan.FromSeconds(60),3 ))
        {
            _settings = settings;
         
        }

        public async Task<List<EstimatedRateResult>> EstimateRate(EstimateRate estimateRate)
        {
            string path = "/v1/rates/estimate";

            var json = JsonConvert.SerializeObject(estimateRate, JsonSerializerSettings);

            return await SendHttpRequestAsync<List<EstimatedRateResult>>(HttpMethod.Post, path, json, _client, _config);
        }

        public async Task<ShipEngineShipment> CreateShipment(ShipEngineShipment shipment)
        {

            string path = $"v1/shipments";


            var shipmentList = new ShipEngineShipmentList
            {
                Shipments = new ShipEngineShipment[] { shipment }
            };

            var json = JsonConvert.SerializeObject(shipmentList, JsonSerializerSettings);

            var result = await SendHttpRequestAsync<ShipEngineShipmentCreationResult>(HttpMethod.Post, path, json, _client, _config);

            return result.Shipments.First();
        }

        public  async Task<ShipmentRateResult> EstimateShipment(string shipmentId)
        {
            string path = $"v1/shipments/{shipmentId}/rates";

            return await SendHttpRequestAsync<ShipmentRateResult>(HttpMethod.Get,path,string.Empty, _client, _config);
        }


        public static ShipEngineClinet Create(ShipmentProviderSetting settings)
        {
            return new ShipEngineClinet(settings);
        }


        public async Task<List<ShipEngineAddressValidationResult>> ValidateAddresses(List<ShipEngineAddress> addresses)
        {
            string jsonContent = JsonConvert.SerializeObject(addresses, JsonSerializerSettings);

            string path = "v1/addresses/validate";

            return await SendHttpRequestAsync<List<ShipEngineAddressValidationResult>>(HttpMethod.Post, path, jsonContent, _client, _config);
        }


    }


    public class ShipEngineShipmentList
    {
        public ShipEngineShipment[] Shipments { get; set; }
    }


 
}
