using Newtonsoft.Json;
using ShipEngineSDK.ValidateAddresses;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Common
{
    public class ShipEngineAddressValidationResult
    {  
        public AddressValidationResult Status { get; set; }

        public ShipEngineAddress OriginalAddress { get; set; }


        [JsonProperty("matched_address")]
        public ShipEngineAddress NormalizedAddress { get; set; }

        public List<Messages> Messages { get; set; }
    }
}
