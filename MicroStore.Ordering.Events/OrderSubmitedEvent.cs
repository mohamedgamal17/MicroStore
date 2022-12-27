using MicroStore.Ordering.Events.Models;

namespace MicroStore.Ordering.Events
{
    public class OrderSubmitedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double Total { get; set; }
        public string UserId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }
}
