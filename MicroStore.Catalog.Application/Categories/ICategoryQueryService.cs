using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Models.Categories;
using MicroStore.Catalog.Entities.ElasticSearch;

namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryQueryService
    {
        Task<Result<List<ElasticCategory>>> ListAsync(CategoryListQueryModel queryParams,CancellationToken cancellationToken = default);
        Task<Result<ElasticCategory>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
