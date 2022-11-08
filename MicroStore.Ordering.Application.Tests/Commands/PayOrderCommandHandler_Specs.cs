using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using MicroStore.BuildingBlocks.InMemoryBus.Contracts;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace MicroStore.Ordering.Application.Tests.Commands
{
    public class PayOrderCommandHandler_Specs : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {

        private readonly ILocalMessageBus _localMessageBus;

        private readonly ICurrentUser _currentUser;

        public PayOrderCommandHandler_Specs()
        {
            _localMessageBus = ServiceProvider.GetRequiredService<ILocalMessageBus>();
            _currentUser = ServiceProvider.GetRequiredService<ICurrentUser>();
        }

        [Test]
        public async Task Should_publish_order_opened_event()
        {

            Guid fakeOrderId = Guid.NewGuid();

            string fakeOrderNumber = Guid.NewGuid().ToString();

            await TestHarness.Bus.Publish(
                  new OrderSubmitedEvent
                  {
                      OrderId = fakeOrderId,
                      OrderNumber = fakeOrderNumber,
                      BillingAddressId = Guid.NewGuid(),
                      ShippingAddressId = Guid.NewGuid(),
                      UserId = _currentUser.Id!.ToString(),
                      SubmissionDate = DateTime.UtcNow,
                      OrderItems = GenerateFakeOrderItems()
                  }
              );
            var instance = await Repository.ShouldContainSagaInState(fakeOrderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

            await _localMessageBus.Send(new PayOrderCommand
            {
                OrderId = fakeOrderId
            });


            Assert.That(await TestHarness.Published.Any<OrderOpenedEvent>());
        }



        [Test]
        public async Task Should_throw_entity_not_found_exception_when_order_is_not_exist()
        {

            Func<Task<PaymentDto>> func = () => _localMessageBus.Send(new PayOrderCommand
            {
                OrderId = Guid.NewGuid()
            });

            await func.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }


        private List<OrderItemModel> GenerateFakeOrderItems()
        {
            return new List<OrderItemModel>
            {

                 new OrderItemModel
                 {
                      ItemName = "FakeName",
                       ProductId = Guid.NewGuid(),
                       Quantity = 5,
                       UnitPrice = 50
                 }
            };
        }

    }
}
