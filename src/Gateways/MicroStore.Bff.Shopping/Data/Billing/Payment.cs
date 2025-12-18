using MicroStore.Bff.Shopping.Data;
namespace MicroStore.Bff.Shopping.Data.Billing
{
    public class Payment : AuditiedEntity<string>
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public double SubTotal { get; set; }
        public double TaxCost { get; set; }
        public double ShippingCost { get; set; }
        public double TotalCost { get; set; }
        public string Description { get; set; }
        public List<PaymentItem> Items { get; set; }
        public string PaymentGateway { get;  set; }
        public string TransctionId { get;  set; }
        public PaymentStatus Status { get;  set; }
        public DateTime? CapturedAt { get;  set; }
        public DateTime? RefundedAt { get; set; }
        public DateTime? FaultAt { get;  set; }
    }

    public enum PaymentStatus
    {
        Waiting = 0,

        Payed = 5,

        UnPayed = 10,

        Refunded = 15,

        Faild = 20
    }
}
