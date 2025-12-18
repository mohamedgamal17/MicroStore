using MicroStore.Catalog.Application.Abstractions.SpecificationAttributes;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Application.Abstractions.Products
{
    public class ProductSpecificationAttributeDto : Entity<string>
    {
        public SpecificationAttributeListDto Attribute { get; set; }
        public SpecificationAttributeOptionDto Option { get; set; }
        public string AttributeId { get; set; }
        public string OptionId { get; set; }
    }
}
