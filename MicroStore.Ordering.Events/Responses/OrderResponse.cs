using MicroStore.Ordering.Events.Models;

namespace MicroStore.Ordering.Events.Responses
{
    public  class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string ShipmentId { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string CurrentState { get; set; }
        public List<OrderItemResponseModel> OrderItems { get; set; } = new List<OrderItemResponseModel>();
    }
}
