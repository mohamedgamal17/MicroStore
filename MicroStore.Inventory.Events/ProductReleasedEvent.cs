using MicroStore.Inventory.Events.Abstractions;

namespace MicroStore.Inventory.Events
{
    public class ProductReleasedEvent : Event
    {
        public int ReleasedQuantity { get; }
        public DateTime ReleasedAt { get;  }

        public ProductReleasedEvent(int releasedQuantity, DateTime releasedAt)
            : base(nameof(ProductReleasedEvent))
        {
            ReleasedQuantity = releasedQuantity;
            ReleasedAt = releasedAt;
        }

    }
}
