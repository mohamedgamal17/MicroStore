using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;

namespace MicroStore.Catalog.Application.Tests.Products.Validation
{
    [TestFixture]
    public class ProductImageCommandValidationTests : BaseTestFixture
    {
        [Test]
        public async Task Should_fail_when_display_order_is_negative()
        {
            var sut = (FakeProductImageCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductImageCommand>>();

            var command = new FakeProductImageCommand
            {
                DisplayOrder = -1,
                ProductId = Guid.NewGuid(),
            };

            var result = await sut.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
        }
    }

    internal class FakeProductImageCommand : ProductImageCommandBase
    {

    }

    internal class FakeProductImageCommandValidator : ProductImageCommandValidatorBase<FakeProductImageCommand> { }
}
