using FluentAssertions;
using MicroStore.Catalog.Application.Models;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Application.Tests.Utilites;
using MicroStore.Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MicroStore.Catalog.Application.Tests.Products
{
    public class When_receiving_create_product_image : ProductCommandTestBase
    {
        [Test]
        public async Task Should_create_product_image_to_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = new CreateProductImageCommand
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

            var productImage = product.ProductImages.Last();

            productImage.DisplayOrder.Should().Be(1);
        }

        [Test]
        public async Task Should_return_error_result_with_404_status_code_while_product_is_not_exist()
        {
            var command = new CreateProductImageCommand
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

    }


    public class When_receiving_update_product_image_command : ProductCommandTestBase
    {
        [Test]
        public async Task Should_update_product_image_to_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = new UpdateProductImageCommand
            {
                ProductImageId = fakeProduct.ProductImages.First().Id,
                ProductId = fakeProduct.Id,

                ImageModel = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}",
                    Data = ImageGenerator.GetBitmapData()
                },

                DisplayOrder = 1
            };

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.IsSuccess.Should().BeTrue();

            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            var productImage = product.ProductImages.Single(x=> x.Id == command.ProductImageId);

            productImage.DisplayOrder.Should().Be(1);
        }


        [Test]
        public async Task Should_return_error_result_with_404_status_code_while_product_is_not_exist()
        {
            var command = new UpdateProductImageCommand
            {
                ProductId = Guid.NewGuid(),
                ProductImageId = Guid.NewGuid(),
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
        public async Task Should_return_error_result_with_404_status_code_exception_while_product_image_is_not_exist()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = new RemoveProductImageCommand
            {
                ProductId = fakeProduct.Id,
                ProductImageId = Guid.NewGuid()
            };
            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
    public class When_receiving_remove_product_image_command : ProductCommandTestBase
    {
        [Test]
        public async Task Should_remove_product_image()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = new RemoveProductImageCommand
            {
                ProductId = fakeProduct.Id,
                ProductImageId = fakeProduct.ProductImages.First().Id
            };


            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.IsSuccess.Should().BeTrue();

            Product product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.ProductImages.Count.Should().Be(0);
        }

        [Test]
        public async Task Should_return_error_result_with_404_status_code_exception_while_product_is_not_exist()
        {
            var command = new RemoveProductImageCommand
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
            var fakeProduct = await CreateFakeProduct();

            var command = new RemoveProductImageCommand
            {
                ProductId = fakeProduct.Id,
                ProductImageId = Guid.NewGuid()
            };
            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
