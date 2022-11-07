namespace MicroStore.Inventory.IntegrationEvents
{
    public class AllocateProductStockIntegationEvent
    {
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
    }
}