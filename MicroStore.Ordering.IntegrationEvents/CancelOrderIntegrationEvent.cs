namespace MicroStore.Ordering.IntegrationEvents
{
    public class CancelOrderIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime CancellationDate { get; set; }
    }
}
