namespace MicroStore.Inventory.IntegrationEvents
{
    public class ReturnProductIntegrationEvent
    {
        public Guid ProductId { get; set; }
        public int ReturnedStock { get; set; }
    }
}
