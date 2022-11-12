namespace MicroStore.Inventory.Domain.Events
{
    public class ProductRecivedEvent : EventBase
    {
        public int RecivedQuantity { get; init; }
        public DateTime RecivedDate { get; init; }
    }
}
