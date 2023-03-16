
using MicroStore.Ordering.Application.Models;

namespace MicroStore.Ordering.Application.StateMachines
{
    public class OrderSubmitedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public AddressModel ShippingAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public string UserName { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
    }

    public class OrderPaymentAcceptedEvent
    {
        public Guid OrderId { get; set; }
        public string PaymentId { get; set; }
        public DateTime PaymentAcceptedDate { get; set; }

    }
    public class OrderApprovedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }

    }

    public class OrderFulfillmentCompletedEvent
    {
        public Guid OrderId { get; set; }
        public string ShipmentId { get; set; }
    }

    public class OrderCompletedEvent
    {
        public Guid OrderId { get; set; }
        public DateTime ShippedDate { get; set; }
    }

    public class OrderCancelledEvent
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime CancellationDate { get; set; }
    }

    public class CheckOrderStatusEvent
    {
        public Guid OrderId { get; set; }
    }
}
