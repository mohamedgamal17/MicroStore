using MicroStore.Bff.Shopping.Data.Common;
namespace MicroStore.Bff.Shopping.Data.Shipping
{
    public class ShipmentItem : Entity<string>
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
