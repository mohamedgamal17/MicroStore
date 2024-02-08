using MicroStore.Bff.Shopping.Data.Billing;
using MicroStore.Bff.Shopping.Data.Profiling;
using MicroStore.Bff.Shopping.Data.Shipping;
namespace MicroStore.Bff.Shopping.Data.Ordering
{
    public class Order : AuditiedEntity<string>
    {
        public string OrderNumber { get; set; }
        public Common.Address ShippingAddress { get; set; }
        public Common.Address BillingAddress { get; set; }
        public UserProfile User { get; set; }
        public Payment? Payment { get; set; }
        public Shipment? Shipment { get; set; }
        public double ShippingCost { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double TotalPrice { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string CurrentState { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public class OrderItem : Entity<string>
    {
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
