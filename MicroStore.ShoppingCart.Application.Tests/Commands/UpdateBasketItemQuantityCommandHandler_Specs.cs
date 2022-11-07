using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using MicroStore.ShoppingCart.Domain.Entities;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace MicroStore.ShoppingCart.Application.Tests.Commands
{
    public class UpdateBasketItemQuantityCommandHandler_Specs : BaseTestBase
    {

        public UpdateBasketItemQuantityCommandHandler_Specs()
        {

        }

        [Test]
        public async Task Should_update_basket_item_quantity()
        {
            var fakeBasket = await CreateFakeBasket();

            UpdateBasketItemQuantityCommand command = new UpdateBasketItemQuantityCommand
            {
                BasketId = fakeBasket.BasketId,
                BasketItemId = fakeBasket.LineItems.First().ItemId,
                Quantity = 5
            };


            await Send(command);


            Basket basket = await WithUnitOfWork((sp) =>
            {
                var repository = ServiceProvider.GetRequiredService<IRepository<Basket>>();
                return repository.SingleAsync(x => x.Id == fakeBasket.BasketId);
            });

            basket.LineItems.Count().Should().Be(1);

            basket.LineItems.First().Quantity.Should().Be(5);

        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_when_basket_is_not_exist()
        {

            UpdateBasketItemQuantityCommand command = new UpdateBasketItemQuantityCommand
            {
                BasketId = Guid.NewGuid(),
                BasketItemId = Guid.NewGuid(),
                Quantity = 5
            };

            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_throw_entity_not_found_exception_when_basket_item_is_not_exist()
        {
            var fakeBasket = await Send(new CreateBasketCommand());

            UpdateBasketItemQuantityCommand command = new UpdateBasketItemQuantityCommand
            {
                BasketId = fakeBasket.BasketId,
                BasketItemId = Guid.NewGuid(),
                Quantity = 5
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
                Quantity = 1
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
