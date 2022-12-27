namespace MicroStore.Catalog.IntegrationEvents
{
    [Obsolete]
    public class AdjustProductNameIntegrationEvent
    {
        public Guid ProductId { get; }
        public string Name { get; }

        public AdjustProductNameIntegrationEvent(Guid productId, string name)
        {
            ProductId = productId;
            Name = name;
        }

    }
}