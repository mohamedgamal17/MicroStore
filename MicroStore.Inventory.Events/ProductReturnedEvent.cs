using MicroStore.Inventory.Events.Abstractions;

namespace MicroStore.Inventory.Events
{
    public class ProductReturnedEvent : Event
    {
       
        public int ReturnedQuantity { get;}

        public DateTime ReturnedDate { get; }

        public ProductReturnedEvent(int returnedQuantity, DateTime returnedDate)
            : base( nameof(ProductReturnedEvent))
        {
            ReturnedQuantity = returnedQuantity;
            ReturnedDate = returnedDate;
        }
    }
}
