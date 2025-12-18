namespace MicroStore.Payment.IntegrationEvents
{
    public class PaymentCreatedIntegrationEvent
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string UserId { get; set; }
    }
}
