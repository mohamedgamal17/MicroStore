namespace MicroStore.Ordering.IntegrationEvents
{
    public class CompleteOrderIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public DateTime ShippedDate { get; set; }
    }
}
