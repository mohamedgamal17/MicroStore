namespace MicroStore.Ordering.Events
{
    public class OrderValidatedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
    }
}
