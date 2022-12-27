namespace MicroStore.Inventory.IntegrationEvents
{
    public class StockRejectedIntegrationEvent
    {
        public string ExternalOrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string ExternalPaymentId { get; set; }
        public string Details { get; set; }
    }
}
