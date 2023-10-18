using MicroStore.Catalog.Entities.ElasticSearch.Common;

namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticProduct : ElasticAuditedEntity
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool IsFeatured { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public ElasticWeight Weight { get; set; }
        public ElasticDimension Dimensions { get; set; }
        public List<ElasticCategory> ProductCategories { get; set; }
        public List<ElasticManufacturer> ProductManufacturers { get; set; }
        public List<ElasticProductImage> ProductImages { get; set; }
        public List<ElasticProductTag> ProductTags { get; set; }
        public List<ElasticProductSpecificationAttribute> SpecificationAttributes { get; set; }

        public ElasticProduct()
        {
            ProductCategories = new List<ElasticCategory>();
            ProductManufacturers = new List<ElasticManufacturer>();
            ProductImages = new List<ElasticProductImage>();
            ProductTags = new List<ElasticProductTag>();
            SpecificationAttributes = new List<ElasticProductSpecificationAttribute>();
        }
    }
}