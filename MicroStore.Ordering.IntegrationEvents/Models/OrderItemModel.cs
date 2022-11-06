namespace MicroStore.Ordering.IntegrationEvents.Models
{
    public class OrderItemModel
    {
        public Guid ProductId { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
