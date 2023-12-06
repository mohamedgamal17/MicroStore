#pragma warning disable CS8618
namespace MicroStore.Inventory.Application.Models
{
    public class ProductModel
    {
        public string  ProductId { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string? Thumbnail { get; set; } 
    }
}
