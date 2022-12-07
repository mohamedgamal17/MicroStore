namespace MicroStore.Payment.IntegrationEvents
{
    public class PaymentAccepetedIntegrationEvent
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentGateway { get; set; }
    }
}
