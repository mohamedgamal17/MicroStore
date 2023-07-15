namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class CategoryRequestOptions
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class CategoryListRequestOptions : SortingRequestOptions
    {
        public string Name { get; set; }
    }
}
