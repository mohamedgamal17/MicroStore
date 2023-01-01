using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ShipEngineSDK.Common.Enums;
namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Common
{
    public class ShipEngineShipmentItem
    {  
        public string? Name { get; set; }

        public string? SalesOrderId { get; set; }

        public string? SalesOrderItemId { get; set; }

        public int? Quantity { get; set; }

        public string? Sku { get; set; }

      
        public string? ExternalOrderId { get; set; }

       
        public string? ExternalOrderItemId { get; set; }

        //
        // Summary:
        //     Amazon Standard Identification Number
        public string? Asin { get; set; }

        //
        // Summary:
        //     The order sources that are supported by ShipEngine
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderSourceCode? OrderSourceCode { get; set; }
    }
}
