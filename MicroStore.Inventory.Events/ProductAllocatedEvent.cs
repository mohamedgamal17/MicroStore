using MicroStore.Inventory.Events.Abstractions;

namespace MicroStore.Inventory.Events
{
    public class ProductAllocatedEvent : Event
    {
        public int AllocatedQuantity { get; }
        public DateTime AllocationDate { get; }

        public ProductAllocatedEvent( int allocatedQuantity, DateTime allocationDate)
            : base(nameof(ProductAllocatedEvent))
        {
            AllocatedQuantity = allocatedQuantity;
            AllocationDate = allocationDate;
        }
    }
}
