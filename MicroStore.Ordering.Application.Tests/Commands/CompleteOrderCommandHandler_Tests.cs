﻿using FluentAssertions;
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
    public class CompleteOrderCommandHandler_Tests : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {

        [Test]
        public async Task Should_complete_order()
        {
            Guid orderId=  Guid.NewGuid();

            await GenerateFakeFullfilledOrder(orderId);

            var command = new CompleteOrderCommand
            {
                OrderId = orderId,
                ShipedDate = DateTime.UtcNow,
            };

            await Send(command);


            Assert.That(await TestHarness.Published.Any<CompleteOrderIntegrationEvent>());
            
        }


        [Test]
        public async Task Should_throw_entity_not_found_exception_while_order_is_not_exist()
        {

            CompleteOrderCommand command = new CompleteOrderCommand
            {
                OrderId = Guid.NewGuid(),
                ShipedDate = DateTime.UtcNow,
            };

            Func<Task> action = () => Send(command);

            await action.Should().ThrowExactlyAsync<EntityNotFoundException>();
        }


        [Test]
        public async Task Should_throw_user_friendly_exception_while_order_state_is_not_in_fullfilled_state()
        {

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            CompleteOrderCommand command = new CompleteOrderCommand
            {
                OrderId = orderId,
                ShipedDate = DateTime.UtcNow,
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

        private async Task GenerateFakeFullfilledOrder(Guid orderId)
        {

            await GenerateFakeSubmitedOrder(orderId);

            await TestHarness.Bus.Publish(new OrderPaymentAcceptedEvent
            {
                OrderId = orderId,
                PaymentId = Guid.NewGuid().ToString(),
                PaymentAcceptedDate = DateTime.UtcNow
            });

            var instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Accepted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();



            await TestHarness.Bus.Publish(
                    new OrderApprovedEvent
                    {
                        OrderId = orderId,
                    }
                );


            instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Approved, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

            await TestHarness.Bus.Publish(new OrderFulfillmentCompletedEvent
            {
                OrderId = orderId,
                ShipmentId = Guid.NewGuid().ToString(),
                ShipmentSystem = Guid.NewGuid().ToString()
            });

            instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Fullfilled, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

        }
    }
}