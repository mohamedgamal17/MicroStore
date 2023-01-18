using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Catalog.Application.Abstractions.Categories.Queries
{
    public class GetCategoryListQuery :SortingQueryParams, IQuery<ListResultDto<CategoryListDto>>
    {
    }
}
