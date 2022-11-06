namespace MicroStore.ShoppingCart.Application.Abstraction.Dtos
{
    public class OrderItemDto
    {
        public string ItemName { get; set; }

        public Guid ProductId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
