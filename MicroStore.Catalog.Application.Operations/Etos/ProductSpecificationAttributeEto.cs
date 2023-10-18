using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class ProductSpecificationAttributeEto : EntityDto<string>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string AttributeId { get; set; }
        public string OptionId { get; set; }
    }
}
