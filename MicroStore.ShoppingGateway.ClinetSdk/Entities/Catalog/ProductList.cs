using MicroStore.ShoppingGateway.ClinetSdk.Common;
namespace MicroStore.ShoppingGateway.ClinetSdk.Entities.Catalog
{
    public class ProductList : BaseEntity<Guid>
    {
        public string Name { get; set; } = null!;
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimensions { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}
