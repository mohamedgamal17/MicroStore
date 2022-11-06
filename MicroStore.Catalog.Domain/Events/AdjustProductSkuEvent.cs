

namespace MicroStore.Catalog.Domain.Events
{
    public class AdjustProductSkuEvent
    {
        public Guid ProductId { get; }
        public string Sku { get; }

        public AdjustProductSkuEvent(Guid productId, string sku)
        {
            ProductId = productId;
            Sku = sku;
        }

    }

}
