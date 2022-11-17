namespace MicroStore.Ordering.Events
{
    public class OrderPaymentCompletedEvent // will modifiy name later
    {
        public Guid OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public string TransactionId { get; set; }
        public DateTime PaymentAcceptedDate { get; set; }

    }
}
