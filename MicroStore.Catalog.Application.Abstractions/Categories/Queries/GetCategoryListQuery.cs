using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;

namespace MicroStore.Catalog.Application.Abstractions.Categories.Queries
{
    public class GetCategoryListQuery :SortingQueryParams, IQuery
    {
    }
}
