namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticSpecificationAttribute : ElasticEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ElasticSpecificationAttributeOption> Options { get; set; }

    }

    public class ElasticSpecificationAttributeOption : ElasticEntity
    {
        public string Value { get; set; }
    }
}
