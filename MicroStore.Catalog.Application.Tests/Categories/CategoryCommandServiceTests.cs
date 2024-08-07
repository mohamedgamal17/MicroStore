﻿using FluentAssertions;
using MicroStore.Catalog.Application.Abstractions.Categories;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
using Volo.Abp.Domain.Entities;
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

            await result.IfSuccess(async (val) =>
            {
                var category = await SingleAsync<Category>(x => x.Id == val.Id);

                category.AssertCategoryModel(model);

                var elasticCategory = await FindElasticDoc<ElasticCategory>(category.Id);

                elasticCategory.Should().NotBeNull();

                elasticCategory!.AssertElasticCategory(category);

            });
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

            await result.IfSuccess(async (val) =>
            {
                var category = await SingleAsync<Category>(x => x.Id == val.Id);

                category.AssertCategoryModel(model);

                var elasticCategory = await FindElasticDoc<ElasticCategory>(category.Id);

                elasticCategory.Should().NotBeNull();

                elasticCategory!.AssertElasticCategory(category);
            });
        }


        [Test]
        public async Task Shoul_failure_while_updating_category_when_category_id_is_not_exist()
        {
            var model = new CategoryModel
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };

            var result = await _categoryCommandService.UpdateAsync(Guid.NewGuid().ToString(), model);

            result.IsFailure.Should().BeTrue();

            result.IfFailure(ex => ex.Should().BeOfType<EntityNotFoundException>());

        }
    }
}
