using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Tests.Utilites;
using MicroStore.Catalog.Domain.Entities;
using System.Net;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products.Commands
{
    public class AssignProductImageCommandHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_assing_image_to_product()
        {
            var fakeProduct = await GenerateFakeProduct();

            var command = new AssignProductImageCommand
            {
                ProductId = fakeProduct.Id,

                ImageModel = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}",
                    Data = ImageGenerator.GetBitmapData()
                },

                DisplayOrder = 1
            };

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            result.IsSuccess.Should().BeTrue();

            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.ProductImages.Count().Should().Be(1);

            var productImage = product.ProductImages[0];

            productImage.DisplayOrder.Should().Be(1);
        }

        [Test]
        public async Task Should_return_error_result_with_404_status_code_while_product_is_not_exist()
        {
            var command = new AssignProductImageCommand
            {
                ProductId = Guid.NewGuid(),
                ImageModel = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}",
                    Data = ImageGenerator.GetBitmapData()
                },
                DisplayOrder = 1
            };



            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


        [Test]
        public async Task Should_return_error_result_with_400_status_code_while_thumbnail_is_not_valid_image()
        {
            var fakeProduct = await GenerateFakeProduct();

            var command = new AssignProductImageCommand
            {
                ProductId = fakeProduct.Id,

                ImageModel = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}",
                    Data = new byte[] { 54, 33, 26, 24, 51, 151, 45 }
                },

                DisplayOrder = 1
            };

            var result = await Send(command);


            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        private Task<Product> GenerateFakeProduct()
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.InsertAsync(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50, Guid.NewGuid().ToString()));
            });
        }





    }
}
