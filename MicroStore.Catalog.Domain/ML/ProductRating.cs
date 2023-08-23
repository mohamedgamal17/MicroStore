namespace MicroStore.Catalog.Domain.ML
{
    public class ProductRating
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public float Rating { get; set; }
    }
}
