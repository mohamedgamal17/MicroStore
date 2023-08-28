namespace MicroStore.Catalog.Application.Operations.Etos
{
    public class ProductReviewEto : EntityEto
    {
        public string UserId { get; set; }

        public string ProductId { get; set; }

        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }

        public string ReplayText { get; set; }
    }
}
