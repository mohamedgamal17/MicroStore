namespace MicroStore.Catalog.Application.Abstractions.Products.Dtos
{
    public class ProductCategoryDto
    {
        public Guid CateogryId { get; set; }
        public string Name { get; set; }
        public bool IsFeaturedProduct { get; set; }

    }
}
