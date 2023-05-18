using MicroStore.BuildingBlocks.Paging;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryQueryService
    {
        Task<Result<List<CategoryDto>>> ListAsync(SortingQueryParams queryParams,CancellationToken cancellationToken = default);
        Task<Result<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
