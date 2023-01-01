#nullable disable
using MicroStore;

namespace MicroStore.Catalog.Api.Models.Products
{
    public class AssignProductCategoryModel
    {
        public Guid CategoryId { get; set; }
        public bool IsFeatured { get; set; }
    }
}
