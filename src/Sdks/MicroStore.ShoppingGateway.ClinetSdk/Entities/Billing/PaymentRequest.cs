namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing
{
    public class PaymentRequest : CreationAuditedEntity<string>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double ShippingCost { get; set; }
        public double TotalCost { get; set; }
        public string TransctionId { get; set; }
        public string PaymentGateway { get; set; }
        public List<PaymentRequestProduct> Items { get; set; }
        public string Description { get; set; }
        public PaymentStatus Status { get;  set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? CapturedAt { get; set; }
    }
}
