using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductSpecificationAttribute : Entity<string>
    {
        public SpecificationAttribute Attribute { get; set; }
        public SpecificationAttributeOption Option { get; set; }

        public string ProductId { get; set; }
        public string AttributeId { get; set; }
        public string OptionId { get; set; }

        public ProductSpecificationAttribute()
        {
            Id= Guid.NewGuid().ToString();
        }
    }
}
