using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Domain.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Dtos
{
    public class ProductDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public string Thumbnail { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimensions { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; }
        public List<ProductImageDto> ProductImages { get; set; }
    }
}
