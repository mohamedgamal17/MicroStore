namespace MicroStore.Bff.Shopping.Data.Billing
{
    public class PaymentProcess
    {
        public string SessionId { get; set; }
        public string TransactionId { get; set; }
        public double AmountSubTotal { get; set; }
        public double AmountTotal { get; set; }
        public string CheckoutLink { get; set; }
        public string CancelUrl { get; set; }
        public string SuccessUrl { get; set; }
        public string Provider { get; set; }
    }
}
