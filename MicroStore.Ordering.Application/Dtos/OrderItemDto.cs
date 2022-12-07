#nullable disable
namespace MicroStore.Ordering.Application.Dtos
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public string ItemName { get; set; }
        public string ProductImage { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
