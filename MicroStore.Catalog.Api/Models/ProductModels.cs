using MicroStore.Catalog.Application.Models;

namespace MicroStore.Catalog.Api.Models
{
    public class ProductModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public ImageModel Thumbnail { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimensions { get; set; }
        public List<CategoryModel> categories { get; set; }
    }

    public class ProductImageModel
    {
        public ImageModel Image { get; set; }
        public int DisplayOrder { get; set; }
    }


}
