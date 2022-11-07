namespace MicroStore.Inventory.IntegrationEvents
{
    public class StockConfirmedIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }
    }
}
