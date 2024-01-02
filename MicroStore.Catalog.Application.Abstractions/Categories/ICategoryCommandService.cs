using MicroStore.BuildingBlocks.Utils.Results;
namespace MicroStore.Catalog.Application.Abstractions.Categories
{
    public interface ICategoryCommandService
    {
        Task<Result<CategoryDto>> CreateAsync(CategoryModel input, CancellationToken cancellationToken = default);
        Task<Result<CategoryDto>> UpdateAsync(string id, CategoryModel input, CancellationToken cancellationToken = default);
    }
}
