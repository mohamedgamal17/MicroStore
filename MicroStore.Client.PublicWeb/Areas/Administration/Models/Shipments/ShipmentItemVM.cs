using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments
{
    public class ShipmentItemVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ProductId { get; set; }
        public string? Thumbnail { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimension { get; set; }
    }
}
