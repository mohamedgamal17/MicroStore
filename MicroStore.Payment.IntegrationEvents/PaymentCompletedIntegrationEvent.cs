namespace MicroStore.Payment.IntegrationEvents
{
    public class PaymentCompletedIntegrationEvent
    {
        public string PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }

        public string CustomerId { get; set; }
        public DateTime PaymentCompletionDate { get; set; }
    }
}
