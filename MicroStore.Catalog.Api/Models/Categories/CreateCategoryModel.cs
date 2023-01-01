#nullable disable
using MicroStore;

namespace MicroStore.Catalog.Api.Models.Categories
{
    public class CreateCategoryModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
