namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticProductSpecificationAttribute : ElasticEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string AttributeId { get; set; }
        public string OptionId { get; set; }
    }
}
