#pragma warning disable CS8618

namespace MicroStore.Catalog.Application.Models.Products
{
    public class CreateProductImageModel
    {
        public string Image { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class UpdateProductImageModel
    {
        public int DisplayOrder { get; set; }
    }

}
