namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    public class CategoryList : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
