using MicroStore.Bff.Shopping.Data.Common;
namespace MicroStore.Bff.Shopping.Data.Shipping
{
    public class Shipment :AuditiedEntity<string>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public Address Address { get; set; }
        public string ShipmentExternalId { get; set; } = string.Empty;
        public string ShipmentLabelExternalId { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public string SystemName { get; set; } = string.Empty;
        public ShipmentStatus Status { get; set; }
        public List<ShipmentItem> Items { get; set; } = new List<ShipmentItem>();
    }

    public enum ShipmentStatus
    {
        Created = 0,

        Fullfilled = 5,

        Shipping = 10,

        Completed = 15
    }
}
