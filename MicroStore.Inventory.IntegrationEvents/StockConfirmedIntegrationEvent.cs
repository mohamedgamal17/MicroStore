namespace MicroStore.Inventory.IntegrationEvents
{
    public class StockConfirmedIntegrationEvent
    {
        public string ExternalOrderId { get; set; }
        public string OrderNumber { get; set; }
        public string ExternalPaymentId { get; set; }
        public string UserId { get; set; }
    }
}
