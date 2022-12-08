﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Tests.Utilites;
using MicroStore.Catalog.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products
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
                    FileName = $"{Guid.NewGuid()}.jpg",
                    Type = "jpg",
                    Data =ImageGenerator.GetBitmapData()
                },

                DisplayOrder = 1
            };

            await Send(command);


            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.ProductImages.Count().Should().Be(1);

            var productImage = product.ProductImages[0];

            productImage.DisplayOrder.Should().Be(1);
        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_while_product_is_not_exist()
        {
            var command = new AssignProductImageCommand
            {
                ProductId = Guid.NewGuid(),
                ImageModel = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}.png",
                    Type = "png",
                    Data = ImageGenerator.GetBitmapData()
                },
                DisplayOrder = 1
            };


            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }

        private Task<Product> GenerateFakeProduct()
        {
            return WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.InsertAsync(new Product(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 50));
            });
        }





    }
}