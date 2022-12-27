using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Models;
using MicroStore.Catalog.Domain.Entities;

using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products.Validation
{
    public class ProductCommandValidationTest : BaseTestFixture
    {

        [Test]
        [TestCase(null, TestName = "ShouldFailWhenProductNameIsNullOrEmpty")]
        [TestCase("AB", TestName = "ShouldFailWhenProductNameIsNotMetMinmumLenght")]
        public async Task ShouldFailIfNameValidationIsNotCorrect(string name)
        {
            await WithUnitOfWork(async (sp) =>
            {
                var sut = (FakeProductCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductCommand>>();

                var command = CreateProductCommand();

                command.Name = name;

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            });

        }


        [Test]
        [TestCase(null, TestName = "ShouldFailWhenProductSkuIsNullOrEmpty")]
        [TestCase("AB", TestName = "ShouldFailWhenProductSkuIsNotMetMinmumLenght")]
        public async Task ShouldFailIfSkuValidationIsNotCorrect(string sku)
        {

            await WithUnitOfWork(async (sp) =>
            {
                var sut = (FakeProductCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductCommand>>();

                var command = CreateProductCommand();

                command.Sku = sku;

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);
            });

        }

        [Test]
        public async Task ShouldFailIfShortDescriptionValidationIsNotCorrect()
        {

            await WithUnitOfWork(async (sp) =>
            {
                var sut = (FakeProductCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductCommand>>();

                var command = CreateProductCommand();

                command.ShortDescription = "AA";

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);
            });

        }


        [Test]
        [TestCase(-5, TestName = "ShouldFailWhenProductPriceIsNegativeValue")]
        [TestCase(0, TestName = "ShouldFailWhenProductPriceIsZero")]
        public async Task ShouldFailIfPriceValidationIsNotCorrect(double price)
        {
            await WithUnitOfWork(async (sp) =>
            {
                var sut = (FakeProductCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductCommand>>();

                var command = CreateProductCommand();

                command.Price = price;

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);
            });

        }


        [Test]
        public async Task ShouldFailIfOldPriceValidationIsNotCorrect()
        {
            await WithUnitOfWork(async (sp) =>
            {
                var sut = (FakeProductCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductCommand>>();

                var command = CreateProductCommand();

                command.OldPrice = -50;

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);
            });

        }



        private FakeProductCommand CreateProductCommand()
            => new FakeProductCommand
            {
                Name = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 50,
                OldPrice = 60
            };



        private class FakeProductCommand : ProductCommandBase { }

        private class FakeProductCommandValidator : ProductCommandValidatorBase<FakeProductCommand>
        {
            public FakeProductCommandValidator(IRepository<Category> categoryRepository)
                : base(categoryRepository)
            {

            }
        }

    }
}
