using MicroStore.Inventory.Events.Abstractions;

namespace MicroStore.Inventory.Events
{
    public class ProductShippedEvent : Event
    {
        public int ShippedQuantity { get; }

        public DateTime ShippedDate { get;  }
        public ProductShippedEvent(int quantity, DateTime shippedDate)
            : base(nameof(ProductShippedEvent))
        {
            ShippedQuantity = quantity;
            ShippedDate = shippedDate;
        }
    }
}
