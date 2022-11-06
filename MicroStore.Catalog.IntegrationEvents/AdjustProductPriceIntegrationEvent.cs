

namespace MicroStore.Catalog.IntegrationEvents
{
    public class AdjustProductPriceIntegrationEvent
    {
        public Guid ProductId { get; }

        public decimal Price { get; }

        public AdjustProductPriceIntegrationEvent(Guid productId, decimal price)
        {
            ProductId = productId;
            Price = price;
        }
    }
}
