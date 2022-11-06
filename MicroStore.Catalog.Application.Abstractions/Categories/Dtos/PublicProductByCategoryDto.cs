

namespace MicroStore.Catalog.Application.Abstractions.Categories.Dtos
{
    public class PublicProductByCategoryDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int TrackMethod { get; set; }
        public decimal Price { get; set; }

    }
}
