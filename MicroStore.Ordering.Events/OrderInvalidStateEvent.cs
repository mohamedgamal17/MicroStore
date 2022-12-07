namespace MicroStore.Ordering.Events
{
    public class OrderInvalidStateEvent
    {
        public Guid OrderId { get; set; }
        public string Error { get; set; }

    }
}
