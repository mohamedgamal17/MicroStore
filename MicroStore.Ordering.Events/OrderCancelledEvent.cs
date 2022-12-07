namespace MicroStore.Ordering.Events
{
    public class OrderCancelledEvent
    {
        public Guid OrderId { get; set; }
        public string Reason { get; set; }
        public DateTime CancellationDate { get; set; }
    }
}
