namespace MicroStore.Payment.IntegrationEvents
{
    [Obsolete("Use Create Payment Integration Event Instead")]
    public class AcceptPaymentIntegationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string TransactionId { get; set; }
    }
}