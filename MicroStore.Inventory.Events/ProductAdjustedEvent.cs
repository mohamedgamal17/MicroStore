using MicroStore.Inventory.Events.Abstractions;

namespace MicroStore.Inventory.Events
{
    public class InventoryAdjustedEvent : Event
    {
        public int AdjustedQuantity { get; }
        public string Reason { get;  }
        public DateTime AdkustedDate { get; set; }

        public InventoryAdjustedEvent(int adjustedQuantity, string reason, DateTime adkustedDate)
            : base( nameof(InventoryAdjustedEvent))
        {
            AdjustedQuantity = adjustedQuantity;
            Reason = reason;
            AdkustedDate = adkustedDate;
        }
    }
}
