namespace MicroStore.Ordering.Events
{
    public class OrderPaymentAcceptedEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentAcceptedDate { get; set; }

    }
}
