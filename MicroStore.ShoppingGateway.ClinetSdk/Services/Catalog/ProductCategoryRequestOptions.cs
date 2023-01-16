namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ProductCategoryCreateOptions
    {
        public Guid CategoryId { get; set; }
        public bool IsFeatured { get; set; }
    }

    public class ProductCategoryUpdateOptions
    {
        public bool  IsFeatured { get; set; }
    }
}
