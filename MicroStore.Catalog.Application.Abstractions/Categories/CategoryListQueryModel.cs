using MicroStore.BuildingBlocks.Utils.Paging.Params;

namespace MicroStore.Catalog.Application.Abstractions.Categories
{
    public class CategoryListQueryModel : SortingQueryParams
    {
        public string? Name { get; set; }
    }
}
