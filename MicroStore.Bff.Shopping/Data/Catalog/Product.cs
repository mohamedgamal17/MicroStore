using MicroStore.Bff.Shopping.Data.Common;
namespace MicroStore.Bff.Shopping.Data.Catalog
{
    public class Product : AuditiedEntity<string>
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool IsFeatured { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public Weight Weight { get; set; }
        public Dimension Dimensions { get; set; }
        public List<ProductCategory> Categories { get; set; }
        public List<ProductManufacturer> Manufacturers { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<ProductTag> Tags { get; set; }
    }
    public class ProductImage : Entity<string>
    {
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
