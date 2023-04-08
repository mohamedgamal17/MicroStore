
namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    public class ProductManufacturer : BaseEntity<string>
    {
        public string ProductId { get; set; }
        public string ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}
