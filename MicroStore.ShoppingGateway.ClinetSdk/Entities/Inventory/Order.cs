using MicroStore.ShoppingGateway.ClinetSdk.Common;
namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Inventory
{
    public class Order : BaseEntity<Guid>
    {
        public string ExternalOrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string ExternalPaymentId { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddres { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public bool IsCancelled { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
