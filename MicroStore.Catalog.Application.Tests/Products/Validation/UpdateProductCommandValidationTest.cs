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
        public async Task ShouldFailWhenProductNameIsAlreadyExistInDatabase()
        {
            Product product1 = await CreateFakeProduct();

            Product product2 = await CreateFakeProduct();

            await WithUnitOfWork(async (sp) =>
            {
                var sut = (UpdateProductCommandCommandValidation)ServiceProvider.GetRequiredService<IValidator<UpdateProductCommand>>();

                var command = new UpdateProductCommand
                {
                    ProductId = product1.Id,
                    Sku = Guid.NewGuid().ToString(),
                    Name = product2.Name,
                    Price = 50
                };

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            });
        }




        [Test]
        public async Task ShouldFailWhenProductSkuIsAlreadyExistInDatabase()
        {
            Product product1 = await CreateFakeProduct();

            Product product2 = await CreateFakeProduct();


            await WithUnitOfWork(async (sp) =>
            {
                var sut = (UpdateProductCommandCommandValidation)ServiceProvider.GetRequiredService<IValidator<UpdateProductCommand>>();

                var command = new UpdateProductCommand
                {
                    ProductId = product1.Id,
                    Sku = product2.Sku,
                    Name = Guid.NewGuid().ToString(),
                    Price = 50
                };


                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            });
        }



        private Task<Product> CreateFakeProduct()
        {
            return WithUnitOfWork(async (sp) =>
            {
                var product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, Guid.NewGuid().ToString());
                var repository = sp.GetRequiredService<IRepository<Product>>();
                await repository.InsertAsync(product,true);
                return product;
            });
        }

    }
}
