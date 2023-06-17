using MicroStore.ShoppingGateway.ClinetSdk.Common;

namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Shipping
{
    public class ShipmentCreateRequestOptions
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public Address Address { get; set; }
        public List<ShipmentItemCreateRequestOptions> Items { get; set; }
    }

    public class ShipmentFullfillRequestOptions
    {
        public Weight Weight { get; set; }

        public Dimension Dimension { get; set; }
    }



    public class ShipmentItemCreateRequestOptions
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ProductId { get; set; }
        public string Thumbnail { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimension { get; set; }
    }
}
