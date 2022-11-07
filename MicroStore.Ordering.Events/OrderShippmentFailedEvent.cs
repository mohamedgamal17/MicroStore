namespace MicroStore.Ordering.Events
{
    public class OrderShippmentFailedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string ShippmentId { get; set; }
        public string Reason { get; set; }

        public DateTime FaultDate { get; set; }
    }
}
