namespace MicroStore.Ordering.Events
{
    public class OrderApprovedEvent 
    {

        public Guid OrderId { get; set; }

        public string OrderNumber { get; set; }

    }
}
