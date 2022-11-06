using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Categories.Validation
{
    public class CreateCategoryCommandValidationTest : BaseTestFixture
    {



        [Test]
        public async Task ShouldFailWhenCategoryNameIsExistInDatabase()
        {
            await WithUnitOfWork(async (sp) =>
            {
                var sut = (CreateCategoryCommandValidation)sp
                .GetRequiredService<IValidator<CreateCategoryCommand>>();

                var command = new CreateCategoryCommand { Name = "DublicateName" };

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count.Should().Be(1);

            });
        }


        [SetUp]
        protected async Task SetupBeforeEachTest()
        {
            await CreateFakeCategory(new Category("DublicateName"));
        }


        private Task<Category> CreateFakeCategory(Category category)
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Category>>();
                await repository.InsertAsync(category);
                return category;
            });
        }

    }
}
