namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Billing
{
    public class PaymentRequestList : CreationAuditedEntity<string>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserName { get; set; }
        public double TaxCost { get; set; }
        public double SubTotal { get; set; }
        public double ShippingCost { get; set; }
        public double TotalCost { get; set; }
        public string TransctionId { get; set; }
        public string PaymentGateway { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? CapturedAt { get; set; }
    }
}
