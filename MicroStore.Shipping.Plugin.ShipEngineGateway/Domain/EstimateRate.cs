using ShipEngineSDK.Common;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Domain
{
    public class EstimateRate
    {
        public string[] CarrierIds { get; set; }
        public string FromCountryCode { get; set; }
        public string FromPostalCode { get; set; }
        public string FromCityLocality { get; set; }
        public string FromStateProvince { get; set; }
        public string ToCountryCode { get; set; }
        public string ToPostalCode { get; set; }
        public string ToCityLocality { get; set; }
        public string ToStateProvince { get; set; }
        public Weight Weight { get; set; }
        public Dimensions Dimensions { get; set; }

    }
}
