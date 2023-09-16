namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductReviewRequestOption
    {
        public string Title { get; set; }

        public string ReviewText { get; set; }

        public int Rating { get; set; }
    }


    public class ReplayOnProductReviewRequestOption
    {
        public string ReplayText { get; set; }
    }
}
