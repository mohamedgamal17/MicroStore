namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    [Serializable]
    public class ProductImage : BaseEntity<string>
    {
        public string ProductId { get; set; }
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
