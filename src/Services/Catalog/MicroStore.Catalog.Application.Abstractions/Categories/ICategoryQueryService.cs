using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Abstractions.Categories
{
    public interface ICategoryQueryService
    {
        Task<Result<List<CategoryDto>>> ListAsync(CategoryListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
