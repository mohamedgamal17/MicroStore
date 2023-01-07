using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Tests.Utilites;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products.Commands
{
    public class UpdateProductCommandHandlerTest : BaseTestFixture
    {

        [Test]
        public async Task Should_update_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = new UpdateProductCommand
            {
                ProductId = fakeProduct.Id,
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 120,
                OldPrice = 150,
                Thumbnail = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}",
                    Data = ImageGenerator.GetBitmapData()
                },
                Weight = new WeightModel
                {
                    Value = 50,
                    Unit = "g"
                },

                Dimensions = new DimensionModel
                {
                    Height = 5,
                    Width = 5,
                    Lenght = 5,
                    Unit = "inch"
                }
            };


            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

            result.IsSuccess.Should().BeTrue();

            var product = await Find<Product>(x => x.Id == fakeProduct.Id);


            product.Sku.Should().Be(command.Sku);
            product.Name.Should().Be(command.Name);
            product.ShortDescription.Should().Be(command.ShortDescription);
            product.LongDescription.Should().Be(command.LongDescription);
            product.Price.Should().Be(command.Price);
            product.OldPrice.Should().Be(command.OldPrice);
            product.Weight.Should().Be(command.Weight.AsWeight());
            product.Dimensions.Should().Be(command.Dimensions.AsDimension());
            product.Thumbnail.Should().Contain(command.Thumbnail.FileName);

            Assert.That(await TestHarness.Published.Any<ProductCreatedIntegrationEvent>());
        }




        [Test]
        public async Task Should_return_error_result_with_404_status_code_while_product_is_not_exist()
        {
            var command = new UpdateProductCommand
            {
                ProductId = Guid.NewGuid(),
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 120,
                OldPrice = 150,
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Should_return_error_result_with_400_status_code_while_thumbnail_is_not_valid_image()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = new UpdateProductCommand
            {
                ProductId = fakeProduct.Id,
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 120,
                OldPrice = 150,
                Thumbnail = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}",
                    Data = new byte[] {54,33,26,24,51,151,45}
                },

            };

            var result = await Send(command);

          
            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }



        private Task<Product> CreateFakeProduct()
        {
            return WithUnitOfWork(async (sp) =>
            {
                var fakeProduct = new Product("FakeSku", "FakeName", 50, Guid.NewGuid().ToString());
                var repository = sp.GetRequiredService<IRepository<Product>>();
                await repository.InsertAsync(fakeProduct);
                return fakeProduct;
            });
        }

        private Task<Category> CreateFakeCategory()
        {
            return WithUnitOfWork(async (sp) =>
            {
                var fakeCategory = new Category("FakeCategory");
                var repository = sp.GetRequiredService<IRepository<Category>>();
                await repository.InsertAsync(fakeCategory);
                return fakeCategory;
            });
        }

    }
}
