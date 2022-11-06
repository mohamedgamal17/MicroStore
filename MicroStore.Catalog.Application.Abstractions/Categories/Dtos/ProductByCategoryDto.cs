

namespace MicroStore.Catalog.Application.Abstractions.Categories.Dtos
{
    public class ProductByCategoryDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }


    }
}
