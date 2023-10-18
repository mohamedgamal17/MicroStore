using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductReview : FullAuditedEntity<string>
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string? ReplayText { get; set; }

        public Product Product { get; set; }

        public ProductReview()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void Update(string title,  string reviewText , int rating)
        {
            Title = title;
            ReviewText = reviewText;
            Rating = rating;
        }

        public void Replay(string replayText)
        {
            ReplayText = replayText;
        }
    }
}
