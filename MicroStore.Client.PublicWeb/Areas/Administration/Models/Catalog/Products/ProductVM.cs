using MicroStore.Client.PublicWeb.Areas.Administration.Models.Common;
using System.ComponentModel;
namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductVM
    {
        public string Id { get; set; }

        [DisplayName("Product Name")]
        public string Name { get; set; }

        [DisplayName("Product Sku")]
        public string Sku { get; set; }

        [DisplayName("Product Categories")]
        public List<ProductCategoryVM> Categories { get; set; }

        [DisplayName("Product Images")]

        public List<ProductImageVM> ProductImages { get; set; }

        [DisplayName("Product Short Description")]
        public string ShortDescription { get; set; }

        [DisplayName("Product Long Description")]
        public string LongDescription { get; set; }

        [DisplayName("Is Featured")]
        public bool IsFeatured { get; set; }

        [DisplayName("Product Price")]
        public double Price { get; set; }

        [DisplayName("Product Old Price")]
        public double OldPrice { get; set; }
        public WeightModel? Weight { get; set; }
        public DimensionModel? Dimension { get; set; }

    }
}
