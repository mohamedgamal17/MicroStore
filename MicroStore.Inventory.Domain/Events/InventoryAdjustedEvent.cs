﻿#nullable disable
namespace MicroStore.Inventory.Domain.Events
{
    public class InventoryAdjustedEvent  : EventBase
    {
        public int AdjustedQuantity { get; init; }
        public DateTime AdkustedDate { get; init; }
    
    }
}
