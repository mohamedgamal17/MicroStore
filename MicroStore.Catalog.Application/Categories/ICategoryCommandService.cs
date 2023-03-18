
using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryCommandService
    {
        Task<Result<CategoryDto>> CreateAsync(CategoryModel input, CancellationToken cancellationToken = default);
        Task<Result<CategoryDto>> UpdateAsync(string id, CategoryModel input, CancellationToken cancellationToken = default);
    }
}
