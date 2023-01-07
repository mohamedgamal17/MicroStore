using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products.Validation
{
    internal class CreateProductCommandValidationTests : BaseTestFixture
    {

        [Test]
        public async Task Should_fail_when_product_name_is_already_exist_in_data_base()
        {

            var fakeProduct = await CreateFakeProduct();

            await WithUnitOfWork(async (sp) =>
            {
                var sut = (CreateProductCommandValidation)ServiceProvider.GetRequiredService<IValidator<CreateProductCommand>>();

                var command = CreateProductCommand();

                command.Name = fakeProduct.Name;

                var result = await sut.ValidateAsync(command);

                result.IsValid.Should().BeFalse();

                result.Errors.Count().Should().Be(1);

            });
        }


        [Test]
        public async Task Should_fail_when_product_sku_is_already_exist_in_data_base()
        {
            var fakeProduct = await CreateFakeProduct();

            await WithUnitOfWork(async (sp) =>
            {
                var sut = (CreateProductCommandValidation)ServiceProvider.GetRequiredService<IValidator<CreateProductCommand>>();

                var command = CreateProductCommand();

                command.Sku = fakeProduct.Sku;

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

     

        private Task<Product> CreateFakeProduct()
        {
            return WithUnitOfWork(async (sp) =>
            {
                var fakeProduct = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, Guid.NewGuid().ToString());
                var repository = sp.GetRequiredService<IRepository<Product>>();
                await repository.InsertAsync(fakeProduct);
                return fakeProduct;
            });
        }
    }
}
