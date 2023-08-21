namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class SpecificationAttribute : ElasticEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SpecificationAttributeOption> Options { get; set; }

    }

    public class SpecificationAttributeOption
    {
        public string Value { get; set; }
    }
}
