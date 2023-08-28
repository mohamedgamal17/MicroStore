using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class SpecificationAttributeEto : EntityEto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SpecificationAttributeOptionEto> Options { get; set; }
    }

    public class SpecificationAttributeOptionEto : EntityEto
    {
        public string Value { get; set; }
    }
}
