using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;

namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryQueryService
    {
        Task<UnitResultV2<List<CategoryListDto>>> ListAsync(SortingQueryParams queryParams,CancellationToken cancellationToken = default);
        Task<UnitResultV2<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
