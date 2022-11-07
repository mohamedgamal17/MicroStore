using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.ShoppingCart.Application.Tests.Commands
{
    public class AddBasketItemCommandHandler_Specs : BaseTestBase
    {

        public AddBasketItemCommandHandler_Specs()
        {

        }

        [Test]
        public async Task Should_add_new_basket_item()
        {
            var fakeBasket = await Send(new CreateBasketCommand());

            var fakeProduct = await InsertFakeProduct();

            AddBasketItemCommand command = new AddBasketItemCommand
            {
                BasketId = fakeBasket.BasketId,
                ProductId = fakeProduct.Id,
                Quantity = 4
            };

            await Send(command);

            Basket basket = await WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Basket>>();

                return repository.SingleAsync(x => x.Id == fakeBasket.BasketId);
            });

            basket.LineItems.First().Quantity.Should().Be(command.Quantity);

            basket.LineItems.Count().Should().Be(1);

        }


        [Test]
        public async Task Should_throw_entity_not_found_exception_when_basket_is_not_exist()
        {
            Func<Task> func = () => Send(new AddBasketItemCommand
            {
                BasketId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),

            });

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_throw_entity_not_found_exception_when_product_is_not_exist()
        {
            var fakeBasket = await Send(new CreateBasketCommand());

            Func<Task> func = () => Send(new AddBasketItemCommand
            {
                BasketId = fakeBasket.BasketId,
                ProductId = Guid.NewGuid(),
            });

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }


        private Task<Product> InsertFakeProduct() => WithUnitOfWork(async (sp) =>
        {
            var repository = sp.GetRequiredService<IRepository<Product>>();
            var product = new Product(Guid.NewGuid(), "FakeName", "FakeSku", 50);
            await repository.InsertAsync(product);
            return product;
        });



    }
}
