namespace MicroStore.Catalog.Application.Abstractions.Products.Dtos
{
    public class ProductImageDto
    {
        public Guid ProductImageId { get; set; }
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
