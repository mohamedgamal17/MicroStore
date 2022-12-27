using MicroStore.Ordering.Events.Models;

namespace MicroStore.Ordering.Events.Responses
{
    public class OrderSubmitedResponse
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddressId { get; set; }
        public string UserId { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxCost { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string CurrentState { get; set; }
        public List<OrderItemResponseModel> OrderItems { get; set; } = new List<OrderItemResponseModel>();
    }
}
