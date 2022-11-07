namespace MicroStore.Inventory.IntegrationEvents
{
    public class RelaseProductStockIntegrationEvent
    {
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
    }
}
