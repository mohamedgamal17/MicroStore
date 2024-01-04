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
        public List<ElasticProductCategory> Categories { get; set; }
        public List<ElasticProductManufacturer> Manufacturers { get; set; }
        public List<ElasticProductImage> ProductImages { get; set; }
        public List<ElasticProductTag> ProductTags { get; set; }
        public List<ElasticProductSpecificationAttribute> SpecificationAttributes { get; set; }

        public ElasticProduct()
        {
            Categories = new List<ElasticProductCategory>();
            Manufacturers = new List<ElasticProductManufacturer>();
            ProductImages = new List<ElasticProductImage>();
            ProductTags = new List<ElasticProductTag>();
            SpecificationAttributes = new List<ElasticProductSpecificationAttribute>();
        }
    }


    public class ElasticProductCategory : ElasticEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ElasticProductManufacturer : ElasticEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}