using MicroStore.ShoppingGateway.ClinetSdk.Entities.Profiling;

namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    public class ProductReview : BaseEntity<string>
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string ReplayText { get; set; }
    }

    public class ProductReviewAggregate : BaseEntity<string>
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public string ProductId { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string ReplayText { get; set; }
    }
}
