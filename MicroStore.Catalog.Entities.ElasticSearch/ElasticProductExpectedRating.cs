namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticProductExpectedRating
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; }

        public float Score { get; set; }
    }
}
