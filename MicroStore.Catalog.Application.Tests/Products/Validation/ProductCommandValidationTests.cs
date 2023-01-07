using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Common;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
namespace MicroStore.Catalog.Application.Tests.Products.Validation
{
    public class ProductCommandValidationTests : BaseTestFixture
    {

        [Test]
        [TestCase(null, TestName = "ShouldFailWhenProductNameIsNullOrEmpty")]
        [TestCase("AB", TestName = "ShouldFailWhenProductNameIsNotMetMinmumLenght")]
        public async Task Should_fail_when_product_Name_is_not_valid(string name)
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
        public async Task Should_fail_when_product_sku_is_not_valid(string sku)
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
        public async Task Should_fail_when_product_short_description_is_not_valid()
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
        public async Task Should_fail_when_product_price_is_not_valid(double price)
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
        public async Task Should_fail_when_product_old_price_is_not_valid()
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

        [TestCase(-50,"g")]
        [TestCase(50,"Unknown")]
        public async Task Should_fail_when_product_weight_value_not_valid(double value ,string unit)
        {
            var sut = (FakeProductCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductCommand>>();

            var command = CreateProductCommand();

            command.Weight = new WeightModel
            {
                Value = value,
                Unit = unit
            };

            var result = await sut.ValidateAsync(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count().Should().Be(1);
        }

        [TestCase(-1,0,0,"inch")]
        [TestCase(0,-1,0,"inch")]
        [TestCase(0,0,-1,"inch")]
        [TestCase(0,0,0,"unknown")]
        public async Task Should_fail_when_product_when_product_dimension_is_not_valid(double lenght, double width , double height, string unit)
        {
            var sut = (FakeProductCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductCommand>>();

            var command = CreateProductCommand();

            command.Dimensions = new DimensionModel
            {
                Lenght = lenght,
                Width = width,
                Height = height,
                Unit = unit
            };

            var result = await sut.ValidateAsync(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
        }

        [Test]
        public async Task Should_fail_when_image_lenght_is_not_valid()
        {
            var sut = (FakeProductCommandValidator)ServiceProvider.GetRequiredService<IValidator<FakeProductCommand>>();

            var command = CreateProductCommand();

            command.Thumbnail = new ImageModel
            {
                Data = new byte[1024 * 1024],
                FileName = Guid.NewGuid().ToString()
            };

            var result = await sut.ValidateAsync(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
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
            public FakeProductCommandValidator(IImageService imageService) : base(imageService)
            {
            }
        }

    }
}
