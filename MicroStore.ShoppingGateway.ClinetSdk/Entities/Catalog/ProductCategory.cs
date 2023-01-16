namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    [Serializable]
    public class ProductCategory : BaseEntity<Guid>
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsFeatured { get; set; }
    }
}
