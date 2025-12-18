namespace MicroStore.Client.PublicWeb.Models
{
    public class PaymentSessionModel
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentRequestId { get; set; }
        public string SessionId { get; set; }
        public string CheckoutUrl { get; set; }
        public string Provider { get; set; }
        public PaymentSessionStatus Status { get; set; }
    }

    public enum PaymentSessionStatus
    {
        Waiting = 0,
        Completed = 5
    }
}
