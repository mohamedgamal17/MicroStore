namespace MicroStore.Ordering.Events
{
    public class OrderCompletedEvent
    {
        public Guid OrderId { get; set; }
        public DateTime ShippedDate { get; set; }
    }
}
