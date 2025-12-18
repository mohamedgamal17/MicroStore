namespace MicroStore.Catalog.Entities.ElasticSearch
{
    public class ElasticProductReview : ElasticAuditedEntity
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string ReplayText { get; set; }
    }
}
