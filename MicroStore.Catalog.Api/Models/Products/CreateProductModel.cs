#nullable disable
using MicroStore;
using MicroStore.Catalog.Application.Abstractions.Common.Models;

namespace MicroStore.Catalog.Api.Models.Products
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double Price { get; set; }
        public double OldPrice { get; set; }
        public IFormFile Thumbnail { get; set; }
        public WeightModel Weight { get; set; }
        public DimensionModel Dimensions { get; set; }
    }
}
