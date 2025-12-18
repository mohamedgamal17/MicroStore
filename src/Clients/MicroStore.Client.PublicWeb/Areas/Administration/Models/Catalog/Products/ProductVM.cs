using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;
using System.ComponentModel;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public List<ProductCategoryVM> Categories { get; set; }
        public List<ProductImageVM> ProductImages { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool IsFeatured { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public WeightModel? Weight { get; set; }
        public DimensionModel? Dimension { get; set; }

    }
}
