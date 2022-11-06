#nullable disable
namespace MicroStore.Catalog.Api.Administration.Models
{
    public class CreateProductCategoryModel
    {
        public Guid ProductId { get; set; }

        public Guid CategoryId { get; set; }
    }
}
