

using MicroStore.Inventory.Domain.Events;

namespace MicroStore.Inventory.Domain.Events
{
    public class ProductAllocatedEvent : EventBase
    {
        public int AllocatedQuantity { get; init;  }
        public DateTime AllocationDate { get; init; }
        
    }
}
