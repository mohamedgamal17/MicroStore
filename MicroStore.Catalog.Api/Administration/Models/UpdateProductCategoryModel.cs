#nullable disable
namespace MicroStore.Catalog.Api.Administration.Models
{
    public class UpdateProductCategoryModel
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
