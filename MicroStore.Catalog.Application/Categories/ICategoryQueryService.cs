using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;

namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryQueryService
    {
        Task<ResultV2<List<CategoryDto>>> ListAsync(SortingQueryParams queryParams,CancellationToken cancellationToken = default);
        Task<ResultV2<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
