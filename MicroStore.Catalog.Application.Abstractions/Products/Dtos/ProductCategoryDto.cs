

using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Products.Dtos
{
    public class ProductCategoryDto
    {
        public Guid ProductCategoryId { get; set; }
        public CategoryDto Category { get; set; } = null!;
        public bool IsFeaturedProduct { get; set; }
    }
}
