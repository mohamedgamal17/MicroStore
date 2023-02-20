using MicroStore.BuildingBlocks.Results;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
namespace MicroStore.Catalog.Application.Categories
{
    public interface ICategoryCommandService
    {
        Task<UnitResultV2<CategoryDto>> CreateAsync(CategoryModel input, CancellationToken cancellationToken = default);
        Task<UnitResultV2<CategoryDto>> UpdateAsync(string id, CategoryModel input, CancellationToken cancellationToken = default);
    }
}
