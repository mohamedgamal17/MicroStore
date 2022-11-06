using MicroStore.Ordering.IntegrationEvents.Models;
namespace MicroStore.Ordering.IntegrationEvents.Responses
{
    public class OrderSubmitedResponse
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }

        public string Status { get; set; }
        public string UserId { get; set; }
        public Guid BillingAddressId { get; set; }
        public Guid ShippingAddressId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public List<OrderItemModel> OrderItemModels { get; set; }
    }
}
