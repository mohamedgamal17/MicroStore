using FluentAssertions;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Inventory.Application.Tests.Commands
{
    public class AdjustProductInventoryCommandHandlerSpecs : BaseTestFixture
    {

        [Test]
        public async Task Should_adjust_product_inventory()
        {

            Product fakeProduct = await Insert(new Product("fakename", "fakesku", 0));

            var result = await Send(new AdjustProductInventoryCommand
            {
                ProductId = fakeProduct.Id,
                Stock = 10
            });

            Product product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.Stock.Should().Be(10);

            result.Stock.Should().Be(10);
        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_when_product_is_not_exist()
        {


            Func<Task> action = () => Send(new AdjustProductInventoryCommand
            {
                ProductId = Guid.NewGuid(),
                Stock = 10
            });

            await action.Should().ThrowExactlyAsync<EntityNotFoundException>();

        }








    }
}
