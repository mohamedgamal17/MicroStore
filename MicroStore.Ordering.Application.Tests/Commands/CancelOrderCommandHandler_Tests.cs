using FluentAssertions;
using MassTransit.Testing;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.IntegrationEvents;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Ordering.Application.Tests.Commands
{
    public class CancelOrderCommandHandler_Tests : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {
        [Test]
        public async Task Should_cancel_order()
        {

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            CancelOrderCommand command = new CancelOrderCommand
            {
                OrderId = orderId,
                CancellationDate = DateTime.UtcNow,
                Reason = Guid.NewGuid().ToString()
            };

            await Send(command);

            Assert.That(await TestHarness.Published.Any<CancelOrderIntegrationEvent>());

        }


        [Test]
        public async Task Should_throw_entity_not_found_exception_while_order_is_not_exist()
        {
            CancelOrderCommand command = new CancelOrderCommand
            {
                OrderId = Guid.NewGuid(),
                CancellationDate = DateTime.UtcNow,
                Reason = Guid.NewGuid().ToString()
            };

            Func<Task> action = () => Send(command);

            await action.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }



        [Test]
        public async Task Should_throw_user_frindely_exception_while_order_is_already_cancelled()
        {
            Guid orderId = Guid.NewGuid();

            await GenerateFakeCancelledOrder(orderId);

            CancelOrderCommand command = new CancelOrderCommand
            {
                OrderId = orderId,
                CancellationDate = DateTime.UtcNow,
                Reason = Guid.NewGuid().ToString()
            };


            Func<Task> action = () => Send(command);


            await action.Should().ThrowExactlyAsync<UserFriendlyException>();
        }

        private async Task GenerateFakeSubmitedOrder(Guid orderId)
        {
            await TestHarness.Bus.Publish(
                  new OrderSubmitedEvent
                  {
                      OrderId = orderId,
                      OrderNumber = Guid.NewGuid().ToString(),
                      BillingAddressId = Guid.NewGuid(),
                      ShippingAddressId = Guid.NewGuid(),
                      TaxCost = 0,
                      ShippingCost = 0,
                      SubTotal = 50,
                      Total = 50,
                      UserId = Guid.NewGuid().ToString(),
                      SubmissionDate = DateTime.UtcNow,
                      OrderItems = new List<OrderItemModel>
                      {
                             new OrderItemModel
                             {
                                  ItemName = Guid.NewGuid().ToString(),
                                  ProductId = Guid.NewGuid(),
                                  Quantity = 5,
                                 UnitPrice = 50
                             }
                      }
                  }
            );

            var instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();
        }


        private async Task GenerateFakeCancelledOrder(Guid orderId)
        {
            await GenerateFakeSubmitedOrder(orderId);

            await TestHarness.Bus.Publish(new OrderCancelledEvent { OrderId = orderId,CancellationDate = DateTime.UtcNow, Reason = Guid.NewGuid().ToString() });

            var instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Cancelled, TestHarness.TestTimeout);

            instance.Should().NotBeNull();
        }


    }
}
