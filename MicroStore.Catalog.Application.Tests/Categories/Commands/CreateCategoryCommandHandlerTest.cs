using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Application.Abstractions.Categories.Dtos;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Categories.Commands
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


            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            result.IsSuccess.Should().BeTrue();

            var category = await GetCategoryById(result.EnvelopeResult.Result.Id);

            category.Name.Should().Be(request.Name);
            category.Description.Should().Be(request.Description);


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
