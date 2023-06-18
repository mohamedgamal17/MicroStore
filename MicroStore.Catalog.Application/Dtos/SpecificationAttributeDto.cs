using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Dtos
{
    public class SpecificationAttributeListDto : EntityDto<string>
    {
        public string Name{ get; set; }
    }
    public class SpecificationAttributeDto : SpecificationAttributeListDto
    {
        public List<SpecificationAttributeOptionDto> Options { get; set; }

    }

    public class SpecificationAttributeOptionDto : EntityDto<string>
    {
        public string Name { get; set; }
        public string AttributeId { get; set; }
    }
}
