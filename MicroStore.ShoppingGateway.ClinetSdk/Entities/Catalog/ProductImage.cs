namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    [Serializable]
    public class ProductImage : BaseEntity<string>
    {
        public string ImagePath { get; set; }
        public int DisplayOrder { get; set; }
    }
}
