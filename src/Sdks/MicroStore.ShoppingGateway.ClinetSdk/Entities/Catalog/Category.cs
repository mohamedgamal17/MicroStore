namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    public class Category : BaseEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
