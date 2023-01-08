using FluentAssertions;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Domain.ProductAggregate;
using System.Net;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Inventory.Application.Tests.Commands
{
    public class AdjustProductInventoryCommandHandlerSpecs : BaseTestFixture
    {

        [Test]
        public async Task Should_adjust_product_inventory()
        {

            Product fakeProduct = await Insert(new Product(Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString(), 0));

            var result = await Send(new AdjustProductInventoryCommand
            {
                Sku = fakeProduct.Sku,
                Stock = 10
            });

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int) HttpStatusCode.OK);

            Product product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.Stock.Should().Be(10);

            result.GetEnvelopeResult<ProductAdjustedInventoryDto>().Result.AdjustedStock.Should().Be(10);
        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_when_product_is_not_exist()
        {


            var result = await  Send(new AdjustProductInventoryCommand
            {
                Sku = Guid.NewGuid().ToString(),
                Stock = 10
            });

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }








    }
}
