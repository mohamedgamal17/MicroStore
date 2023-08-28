using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class ProductEto : EntityEto
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
        public List<CategoryEto> ProductCategories { get; set; }
        public List<ManufacturerEto> ProductManufacturers { get; set; }
        public List<ProductImageEto> ProductImages { get; set; }
        public List<ProductTagEto> ProductTags { get; set; }
        public List<ProductSpecificationAttributeEto> SpecificationAttributes { get; set; }
    }
}
