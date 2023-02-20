#pragma warning disable CS8618
using MicroStore.Catalog.Application.Models;
using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Dtos
{
    public class ProductListDto : EntityDto<string>
    {
        public string Name { get; set; } = null!;
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Thumbnail { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimensions { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; } = null!;
    }
}
