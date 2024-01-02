using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.SpecificationAttributes
{
    public class SpecificationAttributeListDto : FullAuditedEntityDto<string>
    {
        public string Name { get; set; }
    }
}
