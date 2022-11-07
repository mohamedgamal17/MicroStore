using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.Ordering.IntegrationEvents.Responses;
using MicroStore.Ordering.IntegrationEvents;
using MicroStore.ShoppingCart.Application.Abstraction.Commands;
using MicroStore.ShoppingCart.Application.Abstraction.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp;
using MicroStore.ShoppingCart.Domain.Entities;

namespace MicroStore.ShoppingCart.Application.Tests.Commands
{
    public class When_user_try_to_check_out : BaseTestBase
    {

        [Test]
        public async Task Should_publish_submit_order_integration_event()
        {
            var fakeBasket = await CreateFakeBasket();

            CheckoutCommand command = new CheckoutCommand
            {
                BasketId = fakeBasket.BasketId,
                ShippingAddressId = Guid.NewGuid(),
                BillingAddressId = Guid.NewGuid(),
            };

            await Send(command);

            Assert.That(await TestHarness.Published.Any<SubmitOrderIntegrationEvent>());

            Assert.That(await TestHarness.Sent.Any<OrderSubmitedResponse>());
        }

        [Test]
        public async Task Should_throw_user_friendly_exception_when_basket_is_emtpy()
        {
            var fakeBasket = await Send(new CreateBasketCommand());

            CheckoutCommand command = new CheckoutCommand
            {
                BasketId = fakeBasket.BasketId,
                ShippingAddressId = Guid.NewGuid(),
                BillingAddressId = Guid.NewGuid()
            };

            Func<Task> func = () => Send(command);

            await func.Should().ThrowExactlyAsync<UserFriendlyException>();
        }


        [Test]
        public async Task Should_throw_entity_not_found_exception_when_basket_is_not_exist()
        {
            CheckoutCommand command = new CheckoutCommand
            {
                BasketId = Guid.NewGuid(),
                ShippingAddressId = Guid.NewGuid(),
                BillingAddressId = Guid.NewGuid()
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
