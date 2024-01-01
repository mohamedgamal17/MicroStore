using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Catalog.Application.Models.Categories
{
    public class CategoryListQueryModel : SortingQueryParams
    {
        public string? Name { get; set; }
    }
}
