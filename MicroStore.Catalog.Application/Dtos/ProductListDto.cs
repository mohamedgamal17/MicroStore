using MicroStore.Catalog.Domain.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Dtos
{
    public class ProductListDto : EntityDto<Guid>
    {
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
