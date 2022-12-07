namespace MicroStore.Ordering.Events
{
    public class OrderStockRejectedEvent
    {
        public Guid OrderId { get; set; }
        public string Details { get; set; }
    }
}
