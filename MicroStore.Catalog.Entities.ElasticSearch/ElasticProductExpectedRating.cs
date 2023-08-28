namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticProductExpectedRating : ElasticEntity
    {
        public string ProductId { get; set; }
        public string UserId { get; set; }
        public float Score { get; set; }
    }
}
