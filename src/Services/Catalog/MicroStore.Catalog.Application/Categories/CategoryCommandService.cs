using MicroStore.BuildingBlocks.Utils.Results;
using MicroStore.Catalog.Application.Abstractions.Categories;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
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

        public async Task<Result<CategoryDto>> CreateAsync(CategoryModel input, CancellationToken cancellationToken = default)
        {
            Category category = new Category();

            PrepareCategoryEntity(category, input);

            await _categoryRepository.InsertAsync(category, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        public async Task<Result<CategoryDto>> UpdateAsync(string id, CategoryModel input, CancellationToken cancellationToken = default)
        {
            Category? category = await _categoryRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return new Result<CategoryDto>(new EntityNotFoundException(typeof(Category), id)); 

            }
            PrepareCategoryEntity(category, input);

            await _categoryRepository.UpdateAsync(category, cancellationToken: cancellationToken);

            return ObjectMapper.Map<Category, CategoryDto>(category);
        }

        private void PrepareCategoryEntity(Category category, CategoryModel input)
        {
            category.Name = input.Name;
            category.Description = input.Description;
        }

    }
}
