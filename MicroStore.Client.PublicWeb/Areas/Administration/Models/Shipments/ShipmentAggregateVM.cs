using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments
{
    public class ShipmentAggregateVM
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public UserProfileVM User { get; set; }
        public AddressVM Address { get; set; }
        public string ShipmentExternalId { get; set; }
        public string TrackingNumber { get; set; }
        public string ShipmentLabelExternalId { get; set; }
        public string SystemName { get; set; }
        public ShipmentStatus Status { get; set; }
        public List<ShipmentItemVM> Items { get; set; } = new List<ShipmentItemVM>();
        public DateTime? CreatedAt { get; set; }

    }
}
