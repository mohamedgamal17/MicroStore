using MicroStore.Client.PublicWeb.Areas.Administration.Models.Billing;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;
using MicroStore.Client.PublicWeb.Areas.Administration.Models.Shipments;
using MicroStore.ShoppingGateway.ClinetSdk.Common;
using MicroStore.ShoppingGateway.ClinetSdk.Entities.Orderes;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Ordering
{
    public class OrderAggregateVM
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; }
        public OrderState CurrentState { get; set; }
        public AddressVM ShippingAddress { get; set; }
        public AddressVM BillingAddress { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string ShipmentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItemVM> Items { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? CancellationDate { get; set; }
        public PaymentRequestVM Payment { get; set; }
        public ShipmentAggregateVM Shipment { get; set; }
    }
}
