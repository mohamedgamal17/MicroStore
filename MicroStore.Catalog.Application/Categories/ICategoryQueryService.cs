using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models.Categories;
namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryQueryService
    {
        Task<Result<List<CategoryDto>>> ListAsync(CategoryListQueryModel queryParams,CancellationToken cancellationToken = default);
        Task<Result<CategoryDto>> GetAsync(string id, CancellationToken cancellationToken = default);
    }
}
