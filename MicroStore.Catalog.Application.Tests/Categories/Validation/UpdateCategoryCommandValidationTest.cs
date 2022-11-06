using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Categories.Validation
{
    public class UpdateCategoryCommandValidationTest : BaseTestFixture
    {

        [Test]
        public async Task ShouldFailWhenCategoryNameIsExistInDatabase()
        {
            //arrange
            Category fakeCategory = new Category("FakeCategory");

            await CreateFakeCategory(fakeCategory);

            await WithUnitOfWork(async (sp) =>
            {
                var sut = (UpdateCategoryCommandValidation)sp
                .GetRequiredService<IValidator<UpdateCategoryCommand>>();

                var command = new UpdateCategoryCommand { CategoryId = fakeCategory.Id, Name = "DublicateName" };

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();
            });
        }



        [SetUp]
        public async Task SetupBeforeEachTest()
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
