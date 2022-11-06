using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Categories.Commands;


namespace MicroStore.Catalog.Application.Tests.Categories.Validation
{
    public class CategoryCommandValidationTest : BaseTestFixture
    {
        [Test]
        [TestCase(null, Reason = "CategoryNameIsNull")]
        [TestCase("aa", Reason = "CategoryNameLenghtIsNotMetWithMinimumRequiredLenght")]
        public async Task ShouldFailWhenCategoryNameIsNotValid(string name)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var sut = scope.ServiceProvider.GetRequiredService<IValidator<FakeCategoryCommand>>();

                var command = new FakeCategoryCommand
                {
                    Name = name,
                    Description = string.Empty
                };


                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            }

        }

        [Test]
        public async Task ShouldFailWhenCategoryDescriptionIsNotMetWithMinimumRequiredLenght()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var sut = scope.ServiceProvider.GetRequiredService<IValidator<FakeCategoryCommand>>();

                var command = new FakeCategoryCommand
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = "aa"
                };


                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            }

        }



        private class FakeCategoryCommand : CategoryCommandBase
        {

        }


        private class FakeCategoryCommandValidator : CategoryCommandValidatorBase<FakeCategoryCommand>
        {

        }
    }
}
