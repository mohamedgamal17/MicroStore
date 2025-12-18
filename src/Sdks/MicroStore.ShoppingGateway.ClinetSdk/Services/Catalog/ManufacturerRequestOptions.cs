namespace MicroStore.ShoppingGateway.ClinetSdk.Services.Catalog
{
    public class ManufacturerRequestOptions
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ManufacturerListRequestOptions : SortingRequestOptions
    {
        public string Name { get; set; }
    }
}
