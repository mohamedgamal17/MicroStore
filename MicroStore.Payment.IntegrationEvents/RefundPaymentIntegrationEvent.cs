namespace MicroStore.Payment.IntegrationEvents
{
    public class RefundPaymentIntegrationEvent
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string PaymentId { get; set; }

    }
}
