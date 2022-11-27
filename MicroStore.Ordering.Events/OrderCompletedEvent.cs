namespace MicroStore.Ordering.Events
{
    public class OrderCompletedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string ShippmentId { get; set; }
        public DateTime ShippedDate { get; set; }

    }
}
