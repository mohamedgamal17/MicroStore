namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticProductImage : ElasticAuditedEntity
    {
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
        public List<float> Features { get; set; }
    }
}
