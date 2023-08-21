namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticProductImage : ElasticEntity
    {
        public string ProductId { get; set; }
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
