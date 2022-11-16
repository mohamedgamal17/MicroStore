namespace MicroStore.Payment.IntegrationEvents
{
    public class CreatePaymentIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerId { get; set; }

    }
}
