using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MicroStore.Catalog.Application.Tests.Products.Commands
{
    public class UpdateProductImageCommandHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_update_product_image()
        {
            var fakeProduct = await GenerateFakeProduct();

            UpdateProductImageCommand command = new UpdateProductImageCommand
            {
                ProductId = fakeProduct.Id,
                ProductImageId = fakeProduct.ProductImages.First().Id,
                DisplayOrder = 1
            };


            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK );

            result.IsSuccess.Should().BeTrue();


            Product product = await Find<Product>(x => x.Id == fakeProduct.Id);

            ProductImage productImage = product.ProductImages.First();

            productImage.DisplayOrder.Should().Be(command.DisplayOrder);
        }

        [Test]
        public async Task Should_return_error_result_with_404_status_code_exception_while_product_is_not_exist()
        {
            var command = new UpdateProductImageCommand
            {
                ProductId = Guid.NewGuid(),
                ProductImageId = Guid.NewGuid()
            };
            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Should_return_error_result_with_404_status_code_exception_while_product_image_is_not_exist()
        {
            var fakeProduct = await GenerateFakeProduct();

            var command = new UpdateProductImageCommand
            {
                ProductId = fakeProduct.Id,
                ProductImageId = Guid.NewGuid()
            };
            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


        private Task<Product> GenerateFakeProduct()
        {
            return WithUnitOfWork((sp) =>
            {
                Product product = new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, Guid.NewGuid().ToString());

                product.AssignProductImage(Guid.NewGuid().ToString(), 5);

                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.InsertAsync(product);
            });
        }


    }
}
