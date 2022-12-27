namespace MicroStore.Ordering.Events
{
    public class OrderFulfillmentCompletedEvent
    {
        public Guid OrderId { get; set; }
        public string ShipmentId { get; set; }
    }
}
