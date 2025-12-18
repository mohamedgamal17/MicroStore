using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.SpecificationAttributes
{
    public class SpecificationAttributeOptionDto : FullAuditedEntityDto<string>
    {
        public string Name { get; set; }
        public string AttributeId { get; set; }
    }
}
