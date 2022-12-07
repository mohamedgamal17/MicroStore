namespace MicroStore.Ordering.Events
{
    public class CheckOrderStatusEvent 
    {
        public Guid OrderId { get; set; }
    }
}
