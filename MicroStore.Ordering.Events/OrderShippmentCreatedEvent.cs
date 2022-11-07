namespace MicroStore.Ordering.Events
{
    public class OrderShippmentCreatedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string ShippmentId { get; set; }
    }
}
