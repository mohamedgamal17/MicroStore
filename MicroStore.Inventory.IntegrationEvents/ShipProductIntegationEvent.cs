namespace MicroStore.Inventory.IntegrationEvents
{
    public class ShipProductIntegationEvent
    {
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
    }
}
