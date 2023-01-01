using ShipEngineSDK.Common;

namespace MicroStore.Shipping.Plugin.ShipEngineGateway.Domain
{
    public class ShipmentRate
    {
        public string RateId { get; set; }
        public string RateType { get; set; }
        public string CarrierId { get; set; }
        public MonetaryValue InsuranceAmount { get; set; }
        public MonetaryValue ConfirmationAmount { get; set; }
        public MonetaryValue OtherAmount { get; set; }
        public MonetaryValue TaxAmount { get; set; }

        public MonetaryValue ShippingAmount { get; set; }
        public int Zone { get; set; }
        public string PacakgeType { get; set; }
        public int DeliveryDays { get; set; }
        public bool GuaranteedService { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string CarrierDeliveryDays { get; set; }
        public DateTime ShipDate { get; set; }
        public bool NegotiatedRate { get; set; }
        public string ServiceType { get; set; }
        public string ServiceCode { get; set; }
        public bool Trackable { get; set; }
        public string CarrierCode { get; set; }
        public string CarrierNickname { get; set; }
        public string CarrierFriendlyName { get; set; }
        public string ValidationStatus { get; set; }
        public string[] WarningMessages { get; set; }
        public string[] ErrorMessages { get; set; }
    }

    public class ShipmentRateResult
    {
        public ShipmentRate[] Rates { get; set; }
    }
}
