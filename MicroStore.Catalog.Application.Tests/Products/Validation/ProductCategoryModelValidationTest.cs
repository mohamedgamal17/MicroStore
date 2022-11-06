using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Models;

namespace MicroStore.Catalog.Application.Tests.Products.Validation
{
    public class ProductCategoryModelValidationTest : BaseTestFixture
    {


        [Test]
        public async Task ShouldFailWhenCategoryIdIsNotExist()
        {

            await WithUnitOfWork(async (sp) =>
            {
                var sut = (ProductCategoryModelValidator)sp.GetRequiredService<IValidator<ProductCategoryModel>>();

                var model = new ProductCategoryModel
                {
                    CategoryId = Guid.NewGuid(),
                    IsFeatured = false
                };

                var result = await sut.ValidateAsync(model);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);
            });

        }
    }
}
