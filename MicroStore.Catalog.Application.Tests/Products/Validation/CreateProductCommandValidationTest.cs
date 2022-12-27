using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products.Validation
{
    internal class CreateProductCommandValidationTest : BaseTestFixture
    {

        [Test]
        public async Task ShouldWhenProductNameIsAlreadyExistInDatabase()
        {
            await WithUnitOfWork(async (sp) =>
            {
                var sut = (CreateProductCommandValidation)ServiceProvider.GetRequiredService<IValidator<CreateProductCommand>>();

                var command = CreateProductCommand();

                command.Name = "DublicateName";

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            });
        }


        [Test]
        public async Task ShouldWhenProductSkuIsAlreadyExistInDatabase()
        {
            await WithUnitOfWork(async (sp) =>
            {
                var sut = (CreateProductCommandValidation)ServiceProvider.GetRequiredService<IValidator<CreateProductCommand>>();

                var command = CreateProductCommand();

                command.Sku = "DublicateSku";

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            });
        }



        private CreateProductCommand CreateProductCommand()
            => new CreateProductCommand
            {
                Name = Guid.NewGuid().ToString(),
                Sku = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 50,
                OldPrice = 60
            };

        [SetUp]
        protected async Task SetupBeforRunAnyTest()
        {
            await CreateFakeProduct();
        }

        private Task CreateFakeProduct()
        {
            return WithUnitOfWork(async (sp) =>
            {
                var fakeProduct = new Product("DublicateSku", "DublicateName", 50, Guid.NewGuid().ToString());
                var repository = sp.GetRequiredService<IRepository<Product>>();
                await repository.InsertAsync(fakeProduct);
                return fakeProduct;
            });
        }
    }
}
