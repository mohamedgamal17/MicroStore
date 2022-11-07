namespace MicroStore.Payment.IntegrationEvents
{
    public class PaymentRejectedIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string TransactionId { get; set; }

        public DateTime FaultDate { get; set; }
    }
}
