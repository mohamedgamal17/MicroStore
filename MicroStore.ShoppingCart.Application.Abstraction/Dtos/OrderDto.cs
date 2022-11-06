namespace MicroStore.ShoppingCart.Application.Abstraction.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }

        public string OrderNumber { get; set; }

        public string UserId { get; set; }

        public Guid ShippingAddressId { get; set; }

        public Guid BillingAddressId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public DateTime SubmissionDate { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}
