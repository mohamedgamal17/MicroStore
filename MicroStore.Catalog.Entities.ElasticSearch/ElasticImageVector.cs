namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticImageVector
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ImageId { get; set; }
        public List<float> Features { get; set; }
    }

}
