using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Catalog.Application.Models.Categories
{
    public class CategoryListQueryModel : SortingQueryParams
    {
        public string? Name { get; set; }
    }
}
