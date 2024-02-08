namespace MicroStore.Bff.Shopping.Models.Ordering
{
    public class OrderItemModel
    {
        public string ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string? Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
