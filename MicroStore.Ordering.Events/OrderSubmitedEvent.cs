using MicroStore.Ordering.Events.Models;

namespace MicroStore.Ordering.Events
{
    public class OrderSubmitedEvent
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
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
