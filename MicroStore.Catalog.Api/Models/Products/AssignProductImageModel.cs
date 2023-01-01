namespace MicroStore.Catalog.Api.Models.Products
{
    public class AssignProductImageModel
    {
        public IFormFile Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
