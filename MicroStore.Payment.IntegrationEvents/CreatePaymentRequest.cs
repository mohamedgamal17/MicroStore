namespace MicroStore.Payment.IntegrationEvents
{
    public class CreatePaymentRequest
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
