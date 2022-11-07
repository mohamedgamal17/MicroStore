namespace MicroStore.Payment.IntegrationEvents
{
    public class PaymentAccepetedIntegrationEvent
    {
        public Guid OrderId { get; set; }

        public string OrderNumber { get; set; }
        public string TransactionId { get; set; }

        public DateTime PaymentAcceptedDate { get; set; }
    }
}
