using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Abstractions.Categories
{
    public interface ICategoryQueryService
    {
        Task<Result<List<ElasticCategory>>> ListAsync(CategoryListQueryModel queryParams, CancellationToken cancellationToken = default);
        Task<Result<ElasticCategory>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
