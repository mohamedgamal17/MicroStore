
namespace MicroStore.Catalog.Application.Abstractions.Products.Dtos
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; } = null!;
    }
}
