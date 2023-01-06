using MicroStore.Catalog.Domain.ValueObjects;

namespace MicroStore.Catalog.Application.Abstractions.Products.Dtos
{
    public class ProductListDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimensions { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; } = null!;
    }
}
