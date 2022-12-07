using FluentAssertions;
using MicroStore.Inventory.Application.Abstractions.Commands;
using MicroStore.Inventory.Domain.ProductAggregate;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Inventory.Application.Tests.Commands
{
    public class ReciveProductCommandHandlerSpecs : BaseTestFixture
    {
        [Test]
        public async Task Should_recive_product_quantity()
        {
            Product fakeProduct = await Insert(new Product("fakename", "fakesku", 0));

            var result = await Send(new ReciveProductCommand
            {
                ProductId = fakeProduct.Id,
                RecivedQuantity = 5
            });

            Product product = await Find<Product>(x => x.Id == fakeProduct.Id);

            product.Stock.Should().Be(5);

            result.Stock.Should().Be(5);

            result.RecivedStock.Should().Be(5);

        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_when_product_is_not_exist()
        {
            Func<Task> action = () => Send(new ReciveProductCommand
            {
                ProductId = Guid.NewGuid(),
                RecivedQuantity = 5
            });

            await action.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }




    }
}
