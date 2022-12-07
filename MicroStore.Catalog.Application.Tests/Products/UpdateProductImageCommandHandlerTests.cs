using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products
{
    public class UpdateProductImageCommandHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_update_product_image()
        {
            var fakeProduct = await GenerateFakeProduct();

            UpdateProductImageCommand updateProductImageCommand = new UpdateProductImageCommand
            {
                ProductId = fakeProduct.Id,
                ProductImageId = fakeProduct.ProductImages.First().Id,
                DisplayOrder = 1
            };


            await Send(updateProductImageCommand);


            Product product = await Find<Product>(x => x.Id == fakeProduct.Id);

            ProductImage productImage = product.ProductImages.First();

            productImage.DisplayOrder.Should().Be(updateProductImageCommand.DisplayOrder);
        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_while_product_is_not_exist()
        {
            var command = new UpdateProductImageCommand
            {
                ProductId = Guid.NewGuid(),
                ProductImageId = Guid.NewGuid()
            };

            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_while_product_image_is_not_exist()
        {
            var fakeProduct=  await GenerateFakeProduct();

            var command = new UpdateProductImageCommand
            {
                ProductId = fakeProduct.Id,
                ProductImageId = Guid.NewGuid()
            };

            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }


        private Task<Product> GenerateFakeProduct()
        {
            return WithUnitOfWork((sp) =>
            {
                Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50);

                product.AssignProductImage(Guid.NewGuid().ToString(), 5);

                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.InsertAsync(product);
            });
        }


    }
}
