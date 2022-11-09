namespace MicroStore.Payment.IntegrationEvents
{
    public class AcceptPaymentIntegationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string TransactionId { get; set; }
    }
}