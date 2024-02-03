using MicroStore.Bff.Shopping.Models.Common;
namespace MicroStore.Bff.Shopping.Models.Catalog.Products
{
    public class ProductModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimensions { get; set; }
        public HashSet<string>? Categories { get; set; }
        public HashSet<string>? Manufacturers { get; set; }
        public HashSet<string>? ProductTags { get; set; }
        public List<ProductImageModel>? ProductImages { get; set; }
    }
}
