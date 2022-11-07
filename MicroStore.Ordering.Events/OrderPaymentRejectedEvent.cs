namespace MicroStore.Ordering.Events
{
    public class OrderPaymentRejectedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime FaultDate { get; set; }
    }
}
