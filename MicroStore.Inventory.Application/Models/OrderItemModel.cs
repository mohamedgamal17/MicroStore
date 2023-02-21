#pragma warning disable CS8618
namespace MicroStore.Inventory.Application.Models
{
    public class OrderItemModel
    {
        public string ItemId { get; set; }
        public string ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
