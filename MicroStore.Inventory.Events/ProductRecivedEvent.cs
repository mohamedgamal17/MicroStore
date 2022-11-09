using MicroStore.Inventory.Events.Abstractions;

namespace MicroStore.Inventory.Events
{
    public class ProductRecivedEvent : Event
    {
        public int RecivedQuantity { get; }
        public DateTime RecivedDate { get; }
        public ProductRecivedEvent(int recivedQuantity, DateTime recivedDate)
            : base(nameof(ProductRecivedEvent))
        {
            RecivedQuantity = recivedQuantity;
            RecivedDate = recivedDate;
        }
    }
}
