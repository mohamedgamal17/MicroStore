using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;

namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryQueryService
    {
        Task<UnitResult<List<CategoryDto>>> ListAsync(SortingQueryParams queryParams,CancellationToken cancellationToken = default);
        Task<UnitResult<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
