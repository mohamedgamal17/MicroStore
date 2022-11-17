namespace MicroStore.Payment.IntegrationEvents
{
    public class PaymentFaildIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentId { get; set; }
        public string  CustomerId { get; set; }
        public DateTime FaultDate { get; set; }
        public string FaultReason { get; set; }

    }
}
