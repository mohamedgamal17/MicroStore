using FluentAssertions;
using MicroStore.Catalog.Application.Categories;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
namespace MicroStore.Catalog.Application.Tests.Categories
{

    public class CategoryCommandServiceTests : CategoryTestBase
    {
        private readonly ICategoryCommandService _categoryCommandService;

        public CategoryCommandServiceTests()
        {
            _categoryCommandService = GetRequiredService<ICategoryCommandService>();    
        }

        [Test]
        public async Task Should_create_category()
        {
            var model = new CategoryModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };

            var result = await _categoryCommandService.CreateAsync(model);

            result.IsSuccess.Should().BeTrue();

            var category = await Find<Category>(x => x.Id == result.Value.Id);

            category.AssertCategoryModel(model);


        }

        [Test]
        public async Task Should_update_category()
        {

            Category fakeCategory = await CreateFakeCategory();


            var model = new CategoryModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };

            var result = await _categoryCommandService.UpdateAsync(fakeCategory.Id, model);

            result.IsSuccess.Should().BeTrue();

            Category category = await Find<Category>(x => x.Id == fakeCategory.Id);

            category.AssertCategoryModel(model);

        }
    }
}
