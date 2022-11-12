namespace MicroStore.Inventory.Domain.Events
{
    public class ProductShippedEvent : EventBase
    {
        public int ShippedQuantity { get; init; }

        public DateTime ShippedDate { get; init; }
    }
}
