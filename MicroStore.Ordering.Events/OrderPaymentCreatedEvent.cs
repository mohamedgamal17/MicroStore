namespace MicroStore.Ordering.Events
{
    public class OrderPaymentCreatedEvent
    {
        public Guid OrderId { get; set; }
        public string  OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public string PaymentId { get; set; }
    }
}
