using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping
{
    public class ShipmentList
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public Address Address { get; set; }
        public string ShipmentExternalId { get; set; }
        public string TrackingNumber { get; set; }
        public string ShipmentLabelExternalId { get; set; }
        public string SystemName { get; set; }
        public ShipmentStatus Status { get; set; }
    }
}
