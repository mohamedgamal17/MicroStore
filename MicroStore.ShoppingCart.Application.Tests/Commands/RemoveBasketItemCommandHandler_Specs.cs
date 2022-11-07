using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.ShoppingCart.Application.Tests.Commands
{
    public class RemoveBasketItemCommandHandler_Specs : BaseTestBase
    {

        public RemoveBasketItemCommandHandler_Specs()
        {

        }

        [Test]
        public async Task Should_remove_basket_item_from_basket()
        {
            var fakeBasket = await CreateFakeBasket();

            RemoveBasketItemCommand command = new RemoveBasketItemCommand
            {
                BasketId = fakeBasket.BasketId,
                BasketItemId = fakeBasket.LineItems.First().ItemId,
            };


            await Send(command);


            Basket basket = await WithUnitOfWork((sp) =>
            {
                var repository = sp.GetRequiredService<IRepository<Basket>>();

                return repository.SingleAsync(x => x.Id == fakeBasket.BasketId);

            });

            basket.LineItems.Count().Should().Be(0);

        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_when_basket_is_not_exist()
        {

            RemoveBasketItemCommand command = new RemoveBasketItemCommand
            {
                BasketId = Guid.NewGuid(),
            };

            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }




        [Test]
        public async Task Should_throw_entity_not_found_exception_when_basket_item_is_not_exist()
        {
            var fakeBasket = await Send(new CreateBasketCommand());

            RemoveBasketItemCommand command = new RemoveBasketItemCommand
            {
                BasketId = fakeBasket.BasketId,
                BasketItemId = Guid.NewGuid(),

            };

            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();

        }

        private async Task<BasketDto> CreateFakeBasket()
        {
            var fakeBasket = await Send(new CreateBasketCommand());
            var fakeProduct = await InsertFakeProduct();

            return await Send(new AddBasketItemCommand
            {
                BasketId = fakeBasket.BasketId,
                ProductId = fakeProduct.Id,
                Quantity = 5
            });
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
