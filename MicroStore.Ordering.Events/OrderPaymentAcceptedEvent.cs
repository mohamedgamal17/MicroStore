namespace MicroStore.Ordering.Events
{
    public class OrderPaymentAcceptedEvent // will modifiy name later
    {
        public Guid OrderId { get; set; }
        public string PaymentId { get; set; }
        public DateTime PaymentAcceptedDate { get; set; }

    }
}
