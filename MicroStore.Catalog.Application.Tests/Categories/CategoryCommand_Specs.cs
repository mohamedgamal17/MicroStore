using FluentAssertions;
using MicroStore.Catalog.Application.Categories;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
namespace MicroStore.Catalog.Application.Tests.Categories
{
    public abstract class CategoryCommandTestBase : BaseTestFixture
    {
        public async Task<Category> CreateFakeCategory()
        {
            var fakeCategory = new Category
            {
                Name = Guid.NewGuid().ToString(),
            };

            return await Insert(fakeCategory);

        }
    }
    public class When_receiving_create_category_command : CategoryCommandTestBase
    {

        [Test]
        public async Task Should_create_category()
        {
            var request = new CreateCategoryCommand
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString()
            };


            var result = await Send(request);


            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            result.IsSuccess.Should().BeTrue();

            var category = await Find<Category>(x=> x.Id == result.EnvelopeResult.Result.Id);

            category.AssertCategoryCommand(request);


        }
    }


    public class When_receiving_update_category_command : CategoryCommandTestBase
    {
        [Test]
        public async Task Should_update_category()
        {

            Category fakeCategory = await CreateFakeCategory();


            var command = new UpdateCategoryCommand
            {
                CategoryId = fakeCategory.Id,
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };


            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.IsSuccess.Should().BeTrue();


            Category category = await Find<Category>(x=> x.Id == fakeCategory.Id);

            category.AssertCategoryCommand(command);

        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_not_found_while_category_is_not_exist()
        {
            var command = new UpdateCategoryCommand
            {
                CategoryId = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
