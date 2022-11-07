namespace MicroStore.Ordering.Events
{
    public class OrderAcceptedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime AcceptedDate { get; set; }
    }
}
