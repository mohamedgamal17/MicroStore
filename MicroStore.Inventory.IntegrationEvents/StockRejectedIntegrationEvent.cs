namespace MicroStore.Inventory.IntegrationEvents
{
    public class StockRejectedIntegrationEvent
    {
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string UserId { get; set; }
        public string PaymentId { get; set; }
        public Dictionary<string,string> Details { get; set; }
    }
}
