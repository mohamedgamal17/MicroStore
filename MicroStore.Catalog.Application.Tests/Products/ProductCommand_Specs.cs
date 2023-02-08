using FluentAssertions;
using MicroStore.Catalog.Application.Products;
using MicroStore.Catalog.Application.Tests.Extensions;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.IntegrationEvents;
using System.Net;
namespace MicroStore.Catalog.Application.Tests.Products
{


    public class When_receiving_create_product_command : ProductCommandTestBase
    {
        [Test]
        public async Task ShouldCreateProduct()
        {
            var request =  await GenerateCreateProductCommand();

            var result = await Send(request);

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

            var product = await Find<Product>(x=> x.Id == result.EnvelopeResult.Result.Id);

            product.AssertProductCommand(request);

            Assert.That(await TestHarness.Published.Any<ProductCreatedIntegrationEvent>());

        }
    }

    public class When_receiving_update_product_command : ProductCommandTestBase
    {
        [Test]
        public async Task Should_update_product()
        {
            var fakeProduct = await CreateFakeProduct();

            var command = (UpdateProductCommand) await GenerateUpdateProductCommand();

            command.ProductId = fakeProduct.Id;
       
            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.IsSuccess.Should().BeTrue();

            var product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.AssertProductCommand(command);

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
    }


}
