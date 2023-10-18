using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class SpecificationAttributeEto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SpecificationAttributeOptionEto> Options { get; set; }
    }

    public class SpecificationAttributeOptionEto : EntityDto<string>
    {
        public string Value { get; set; }
    }
}
