namespace MicroStore.Catalog.Api.Administration.Models.Products
{
    public class AssignProductImageModel
    {
        public IFormFile Image { get; set; }
        public int DisplayOrder { get; set; }
    }
}
