namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    [Serializable]
    public class ProductCategory : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
