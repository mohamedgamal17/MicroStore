#nullable disable
namespace MicroStore.Ordering.Application.StateMachines
{
    public class OrderItemEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ItemName { get; set; }
        public string ProductImage { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
