namespace MicroStore.Inventory.IntegrationEvents
{
    public class StockConfirmedIntegrationEvent
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string PaymentId { get; set; }
        public string UserId { get; set; }
    }
}
