using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Categories
{
    public class CreateCategoryCommandHandlerTest : BaseTestFixture
    {

        [Test]
        public async Task ShouldCreateNewCategory()
        {
            var request = new CreateCategoryCommand
            {
                Name = "FakeCategory",
                Description = "FakeDescription"
            };


            var result = await Send(request);
            var categoriesCount = await GetCategoriesCount();
            var category = await GetCategoryById(result.CategoryId);

            category.Name.Should().Be(request.Name);
            category.Description.Should().Be(request.Description);
            categoriesCount.Should().Be(1);

        }



        private Task<int> GetCategoriesCount()
        {
            return WithUnitOfWork(sp =>
            {
                var repository = sp.GetRequiredService<IRepository<Category>>();
                return repository.CountAsync();
            });
        }

        private Task<Category> GetCategoryById(Guid id)
        {
            return WithUnitOfWork(sp =>
            {
                var repository = sp.GetRequiredService<IRepository<Category>>();
                return repository.SingleAsync(x => x.Id == id);
            });
        }
    }
}
