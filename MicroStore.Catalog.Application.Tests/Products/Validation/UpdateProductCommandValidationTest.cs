using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products.Validation
{
    public class UpdateProductCommandValidationTest : BaseTestFixture
    {
        [Test]
        public async Task ShouldWhenProductNameIsAlreadyExistInDatabase()
        {
            Product fakeProduct = await CreateFakeProduct(new Product("FakeSku", "FakeName", 50));


            await WithUnitOfWork(async (sp) =>
            {
                var sut = (UpdateProductCommandCommandValidation)ServiceProvider.GetRequiredService<IValidator<UpdateProductCommand>>();

                var command = new UpdateProductCommand
                {
                    ProductId = fakeProduct.Id,
                    Sku = Guid.NewGuid().ToString(),
                    Name = "DublicateName",
                    Price = 50
                };

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            });
        }




        [Test]
        public async Task ShouldWhenProductSkuIsAlreadyExistInDatabase()
        {

            Product fakeProduct = await CreateFakeProduct(new Product("FakeSku", "FakeName", 50));


            await WithUnitOfWork(async (sp) =>
            {
                var sut = (UpdateProductCommandCommandValidation)ServiceProvider.GetRequiredService<IValidator<UpdateProductCommand>>();

                var command = new UpdateProductCommand
                {
                    ProductId = fakeProduct.Id,
                    Sku = "DublicateSku",
                    Name = Guid.NewGuid().ToString(),
                    Price = 50
                };


                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            });
        }



        private Task<Product> CreateFakeProduct(Product product)
        {
            return WithUnitOfWork(async (sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();
                await repository.InsertAsync(product);
                return product;
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
            await CreateFakeProduct(new Product("DublicateSku", "DublicateName", 50));
        }
    }
}
