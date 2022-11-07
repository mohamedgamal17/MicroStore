namespace MicroStore.Ordering.Events
{
    public class OrderOpenedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string TransactionId { get; set; }
    }
}
