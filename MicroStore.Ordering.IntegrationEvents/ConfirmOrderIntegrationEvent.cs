namespace MicroStore.Ordering.IntegrationEvents
{
    public class ConfirmOrderIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime ConfirmationDate { get; set; }
    }
}
