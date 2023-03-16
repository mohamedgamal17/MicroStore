namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    [Serializable]
    public class ProductCategory : BaseEntity<string>
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
    }
}
