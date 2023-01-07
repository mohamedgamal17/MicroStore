using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;

namespace MicroStore.Catalog.Application.Tests.Products.Validation
{
    public class AssignProductImageCommandValidationTests : BaseTestFixture
    {
        [Test]
        public async Task Should_fail_when_image_length_is_not_valid()
        {
            var sut = (AssignProductImageCommandValidator)ServiceProvider
                .GetRequiredService<IValidator<AssignProductImageCommand>>();

            var command = new AssignProductImageCommand
            {
                ProductId = Guid.NewGuid(),
                ImageModel = new ImageModel
                {
                    FileName = Guid.NewGuid().ToString(),
                    Data = new byte[1024 * 1024]
                }
            };

            var result = await sut.ValidateAsync(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count().Should().Be(1);
        }
    }
}
