namespace MicroStore.Payment.IntegrationEvents
{
    [Obsolete("Use VoidPaymentIntegrationEvent")]
    public class RefundUserIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public Guid OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public string TransactionId { get; set; }

        public DateTime RefundationDate { get; set; }

    }
}
