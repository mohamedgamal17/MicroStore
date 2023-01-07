using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Categories
{
    public class UpdateCategoryCommandHandlerTest : BaseTestFixture
    {
        [Test]
        public async Task ShouldUpdateCategory()
        {

            Category fakeCategory = await CreateFakeCategory();


            var command = new UpdateCategoryCommand
            {
                CategoryId = fakeCategory.Id,
                Name = "NewName",
                Description = "NewDescription"
            };


            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

            result.IsSuccess.Should().BeTrue();


            Category category = await GetCategoryById(fakeCategory.Id);

            category.Name.Should().Be(command.Name);

            category.Description.Should().Be(command.Description);

        }

        private Task<Category> CreateFakeCategory()
        {
            return WithUnitOfWork(async (sp) =>
            {
                var fakeCategory = new Category("FakeCategory");
                var repository = sp.GetRequiredService<IRepository<Category>>();
                await repository.InsertAsync(fakeCategory);
                return fakeCategory;
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
