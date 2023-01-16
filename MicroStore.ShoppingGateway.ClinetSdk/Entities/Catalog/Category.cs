namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    public class Category : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
