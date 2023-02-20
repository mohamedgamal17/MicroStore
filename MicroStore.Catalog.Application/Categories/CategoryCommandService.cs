using Microsoft.EntityFrameworkCore;
using MicroStore.BuildingBlocks.Results;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Catalog.Application.Dtos;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Categories
{
    public class CategoryCommandService : CatalogApplicationService, ICategoryCommandService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryCommandService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<UnitResultV2<CategoryDto>> CreateAsync(CategoryModel input, CancellationToken cancellationToken = default)
        {
            Category category = new Category();

            PrepareCategoryEntity(category, input);

            await _categoryRepository.InsertAsync(category, cancellationToken: cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<Category, CategoryDto>(category));
        }

        public async Task<UnitResultV2<CategoryDto>> UpdateAsync(string id, CategoryModel input, CancellationToken cancellationToken = default)
        {
            Category? category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return UnitResultV2.Failure<CategoryDto>(ErrorInfo.NotFound($"Category entity with id : {id} is not found"));

            }
            PrepareCategoryEntity(category, input);

            await _categoryRepository.UpdateAsync(category, cancellationToken: cancellationToken);

            return UnitResultV2.Success(ObjectMapper.Map<Category, CategoryDto>(category));
        }

        private void PrepareCategoryEntity(Category category, CategoryModel input)
        {
            category.Name = input.Name;
            category.Description = input.Description;
        }

    }
}
