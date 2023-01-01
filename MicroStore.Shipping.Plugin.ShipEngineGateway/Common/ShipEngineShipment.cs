using ShipEngineSDK.Common.Enums;
using ShipEngineSDK.Common;
using ShipEngineSDK.CreateLabelFromShipmentDetails;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Common
{
    public class ShipEngineShipment
    {
        public string ShipmentId { get; set; }
        public string ShipmentExternalId { get; set; }

        public string CarrierId { get; set; }

        public string ServiceCode { get; set; }

      
        public string ExternalOrderId { get; set; }

       
        public List<ShipEngineShipmentItem> Items { get; set; }

        
        public List<TaxIdentifier> TaxIdentifiers { get; set; }

        
        public string ExternalShipmentId { get; set; }

        
        public string ShipDate { get; set; }

        
        public ShipEngineAddress ShipTo { get; set; }


        public ShipEngineAddress ShipFrom { get; set; }

     
        public string WarehouseId { get; set; }


        public Address ReturnTo { get; set; }

        
        [JsonConverter(typeof(StringEnumConverter))]
        public DeliveryConfirmation Confirmation { get; set; }

       
        public InternationalShipmentOptions Customs { get; set; }

      
        public AdvancedShipmentOptions AdvancedOptions { get; set; }

        
        [JsonConverter(typeof(StringEnumConverter))]
        public OriginType OriginType { get; set; }

        
        [JsonConverter(typeof(StringEnumConverter))]
        public InsuranceProvider InsuranceProvider { get; set; }

       

        public string OrderSourceCode { get; set; }

      
        public List<Package> Packages { get; set; }
    }
}
