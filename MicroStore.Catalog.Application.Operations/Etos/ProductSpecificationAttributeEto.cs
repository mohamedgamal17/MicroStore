namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class ProductSpecificationAttributeEto : EntityEto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string AttributeId { get; set; }
        public string OptionId { get; set; }
    }
}
