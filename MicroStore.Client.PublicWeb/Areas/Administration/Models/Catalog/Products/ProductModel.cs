using System.ComponentModel;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Models.Catalog.Products
{
    public class ProductModel
    {
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [DisplayName("Product Sku")]
        public string Sku { get; set; }

        [DisplayName("Product Thumbnail")]
        public IFormFile Thumbnail { get; set; }

        public string[] CategoriesId { get; set; }

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
