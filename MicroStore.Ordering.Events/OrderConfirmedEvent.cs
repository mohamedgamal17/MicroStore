namespace MicroStore.Ordering.Events
{
    public class OrderConfirmedEvent
    {
        public Guid OrderId { get; set; }

        public DateTime ConfirmationDate { get; set; }
    }
}
