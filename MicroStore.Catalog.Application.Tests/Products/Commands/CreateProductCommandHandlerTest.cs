using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Catalog.Application.Abstractions.Common.Models;
using MicroStore.Catalog.Application.Abstractions.Products.Commands;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Tests.Utilites;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using System.Net;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.Catalog.Application.Tests.Products.Commands
{
    public class CreateProductCommandHandlerTest : BaseTestFixture
    {

        [Test]
        public async Task ShouldCreateProduct()
        {
            var request = new CreateProductCommand
            {
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Thumbnail = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}",
                    Data = ImageGenerator.GetBitmapData()
                },
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 50,
                OldPrice = 150,
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

            var result = await Send(request);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var product = await GetProductById(result.GetEnvelopeResult<ProductDto>().Result.ProductId);

            product.Name.Should().Be(request.Name);
            product.ShortDescription.Should().Be(request.ShortDescription);
            product.LongDescription.Should().Be(request.LongDescription);
            product.Price.Should().Be(request.Price);
            product.OldPrice.Should().Be(request.OldPrice);
            product.Thumbnail.Should().Contain(request.Thumbnail.FileName);
            product.Weight.Should().Be(request.Weight.AsWeight());
            product.Dimensions.Should().Be(request.Dimensions.AsDimension());

            Assert.That(await TestHarness.Published.Any<ProductCreatedIntegrationEvent>());

        }

        [Test]
        public async Task Should_return_error_result_with_400_status_code_when_product_thumbnail_is_not_valid_image()
        {
            var command = new CreateProductCommand
            {
                Sku = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Thumbnail = new ImageModel
                {
                    FileName = $"{Guid.NewGuid()}",
                    Data = new byte[] { 54, 33, 26, 24, 51, 151, 45 }
                },
                ShortDescription = Guid.NewGuid().ToString(),
                LongDescription = Guid.NewGuid().ToString(),
                Price = 50,
                OldPrice = 150,
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
 
        private Task<Product> GetProductById(Guid id)
        {
            return WithUnitOfWork(sp =>
            {
                var repository = sp.GetRequiredService<IRepository<Product>>();

                return repository.SingleAsync(x => x.Id == id);
            });
        }

    }
}
