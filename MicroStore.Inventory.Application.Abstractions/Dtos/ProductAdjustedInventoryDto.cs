namespace MicroStore.Inventory.Application.Abstractions.Dtos
{
    public class ProductAdjustedInventoryDto 
    {
        public Guid ProductId { get; set; }
        public string ExternalProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }

        public string Thumbnail { get; set; }
        public int AdjustedStock { get; set; }
    }
}
