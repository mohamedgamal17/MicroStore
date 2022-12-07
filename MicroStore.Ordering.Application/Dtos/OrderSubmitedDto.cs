#nullable disable
namespace MicroStore.Ordering.Application.Dtos
{
    public class OrderSubmitedDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public Guid ShippingAddressId { get; set; }
        public Guid BillingAddressId { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string UserId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();


    }
}
