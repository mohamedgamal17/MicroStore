namespace MicroStore.Inventory.IntegrationEvents
{
    public class StockRejectedIntegrationEvent
    {
        public Guid OrderId { get; set; }
        public string OrderNubmer { get; set; }
        public string Details { get; set; }
    }
}
