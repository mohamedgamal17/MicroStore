using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments
{
    public class ShipmentVM
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public AddressModel Address { get; set; }
        public string ShipmentExternalId { get; set; }
        public string TrackingNumber { get; set; }
        public string ShipmentLabelExternalId { get; set; }
        public string SystemName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ShipmentStatus Status { get; set; }
        public List<ShipmentItem> Items { get; set; } = new List<ShipmentItem>();
        public DateTime? CreatedAt { get; set; }

    }



}
