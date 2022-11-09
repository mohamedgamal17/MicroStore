using MicroStore.Inventory.Events.Abstractions;

namespace MicroStore.Inventory.Events
{
    public class ProductDispatchedEvent : Event
    {
        public string Name { get;  }
        public string Sku { get; set; }

        public ProductDispatchedEvent(string name, string sku)
            : base(nameof(ProductDispatchedEvent))
        {
            Name = name;
            Sku = sku;
        }
    }
}
