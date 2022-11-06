

namespace MicroStore.Catalog.Application.Abstractions.Categories.Dtos
{
    public class PublicCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<PublicProductByCategoryDto> Prodcuts { get; set; }

    }
}
