using Volo.Abp.Application.Dtos;
namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class ProductEto : FullAuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool IsFeatured { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public WeightEto Weight { get; set; }
        public DimensionEto Dimensions { get; set; }
        public List<ProductCategoryEto> Categories { get; set; }
        public List<ProductManufacturerEto> Manufacturers { get; set; }
        public List<ProductImageEto> ProductImages { get; set; }
        public List<ProductTagEto> Tags { get; set; }
        public List<ProductSpecificationAttributeEto> SpecificationAttributes { get; set; }
    }

    public class ProductCategoryEto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ProductManufacturerEto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
