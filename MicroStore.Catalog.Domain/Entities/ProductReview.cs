using Volo.Abp.Domain.Entities;

namespace MicroStore.Catalog.Domain.Entities
{
    public class ProductReview : Entity<string>
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
