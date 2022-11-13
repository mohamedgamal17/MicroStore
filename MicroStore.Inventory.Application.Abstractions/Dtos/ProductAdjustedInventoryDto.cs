namespace MicroStore.Inventory.Application.Abstractions.Dtos
{
    public class ProductAdjustedInventoryDto 
    {
        public Guid ProductId { get; set; }
        public int Stock { get; set; }
    }
}
