#pragma warning disable CS8618
using MicroStore.Catalog.Domain.ValueObjects;
using Volo.Abp.Domain.Entities.Auditing;
namespace MicroStore.Catalog.Domain.Entities
{
    public class Product : FullAuditedAggregateRoot<string>
    {
        public string Name { get;  set; }
        public string Sku { get;  set; }
        public string ShortDescription { get;set; } = string.Empty;
        public string LongDescription { get;set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimensions { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<ProductSpecificationAttribute> SpecificationAttributes { get; set; } = new List<ProductSpecificationAttribute>();

        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
