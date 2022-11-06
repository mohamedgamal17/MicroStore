

namespace MicroStore.Catalog.IntegrationEvents
{
    public class AdjustProductSkuIntegrationEvent
    {
        public Guid ProductId { get; }
        public string Sku { get; }

        public AdjustProductSkuIntegrationEvent(Guid productId, string sku)
        {
            ProductId = productId;
            Sku = sku;
        }
    }
}
