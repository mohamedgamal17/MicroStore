using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentRateRetrieveRequestOptions
    {
        public string SystemName { get; set; }
        public string ExternalShipmentId { get; set; }
    }

    public class ShipmentRateEstimateRequestOptions
    {
        public Address Address { get; set; }
        public List<ShipmentItemEstimateRequestOptions> Items { get; set; }
    }

    public class ShipmentItemEstimateRequestOptions
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public Money UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Weight Weight { get; set; }
    }
}
