using FluentAssertions;
using MassTransit.Testing;
using MicroStore.Ordering.Application.Abstractions.Commands;
using MicroStore.Ordering.Application.Abstractions.StateMachines;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Ordering.IntegrationEvents;
using System.Net;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Ordering.Application.Tests.Commands
{
    public class FullfillOrderCommandHandler_Tests : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {

        [Test]
        public async Task Should_fullfill_order()
        {
            Guid orderId =  Guid.NewGuid();

            await GenerateFakeApprovedOrder(orderId);


            FullfillOrderCommand command = new FullfillOrderCommand
            {
                OrderId = orderId,
                ShipmentId = Guid.NewGuid().ToString(),
            };

            await Send(command);

            Assert.That(await TestHarness.Published.Any<OrderFulfillmentCompletedEvent>());

        }

        [Test]
        public async Task Should_return_error_result_with_status_code_404_while_order_is_not_exist()
        {
            TestHarness.TestTimeout = TimeSpan.FromSeconds(1);

            FullfillOrderCommand command = new FullfillOrderCommand
            {
                OrderId = Guid.NewGuid(),
                ShipmentId = Guid.NewGuid().ToString(),
            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Should_return_error_result_with_status_code_400_while_order_state_is_not_in_approved_state()
        {
            TestHarness.TestTimeout = TimeSpan.FromSeconds(1);

            Guid orderId = Guid.NewGuid();

            await GenerateFakeSubmitedOrder(orderId);

            FullfillOrderCommand command = new FullfillOrderCommand
            {
                OrderId = orderId,
                ShipmentId = Guid.NewGuid().ToString(),

            };

            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


        private async Task GenerateFakeSubmitedOrder(Guid orderId)
        {
            await TestHarness.Bus.Publish(
                  new OrderSubmitedEvent
                  {
                      OrderId = orderId,
                      OrderNumber = Guid.NewGuid().ToString(),
                      BillingAddress = new AddressModel(),
                      ShippingAddress = new AddressModel(),
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
                                  Name = Guid.NewGuid().ToString(),
                                  ExternalProductId = Guid.NewGuid().ToString(),
                                  Sku =Guid.NewGuid().ToString(),
                                  Quantity = 5,
                                  UnitPrice = 50
                             }
                      }
                  }
              );

            var instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();
        }
        private async Task GenerateFakeApprovedOrder(Guid orderId)
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
        }
    }
}
