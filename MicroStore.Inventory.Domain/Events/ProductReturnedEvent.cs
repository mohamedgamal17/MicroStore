
namespace MicroStore.Inventory.Domain.Events
{
    public class ProductReturnedEvent : EventBase
    {
       
        public int ReturnedQuantity { get; init; }

        public DateTime ReturnedDate { get; init; }
    }
}
