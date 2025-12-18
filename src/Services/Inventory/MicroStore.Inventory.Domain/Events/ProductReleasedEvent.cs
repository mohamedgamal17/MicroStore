namespace MicroStore.Inventory.Domain.Events
{
    public class ProductReleasedEvent : EventBase
    {
        public int ReleasedQuantity { get; init; }
        public DateTime ReleasedAt { get; init; }

    }
}
