using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Shipping;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes
{
    public class OrderAggregate : BaseEntity<Guid>
    {
        public string OrderNumber { get; set; }
        public OrderState CurrentState { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string PaymentId { get; set; }
        public string ShipmentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItem> Items { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? CancellationDate { get; set; }
        public AddressAggregate ShippingAddress { get; set; }
        public AddressAggregate BillingAddress { get; set; }
        public PaymentRequestAggregate Payment { get; set; }
        public ShipmentAggregate Shipment { get; set; }
    }
}
