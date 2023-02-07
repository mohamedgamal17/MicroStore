
using FluentAssertions;
using MassTransit.Testing;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.StateMachines;

namespace MicroStore.Ordering.Application.Tests.Orders
{
    public class OrderCommandTestBase : StateMachineTestFixture<OrderStateMachine,OrderStateEntity>
    {
        public AddressModel GenerateFakeAddress()
        {
            return new AddressModel
            {
                CountryCode = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                State = Guid.NewGuid().ToString(),
                AddressLine1 = Guid.NewGuid().ToString(),
                AddressLine2 = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Phone = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString(),
                Zip = Guid.NewGuid().ToString(),
            };
        }

        public async Task GenerateFakeSubmitedOrder(Guid orderId)
        {
            await TestHarness.Bus.Publish(
                  new OrderSubmitedEvent
                  {
                      OrderId = orderId,
                      OrderNumber = Guid.NewGuid().ToString(),
                      BillingAddress = GenerateFakeAddress(),
                      ShippingAddress = GenerateFakeAddress(),
                      TaxCost = 0,
                      ShippingCost = 0,
                      SubTotal = 50,
                      TotalPrice = 50,
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


        public async Task GenerateFakeAcceptedOrder(Guid orderId)
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

        }

        public async Task GenerateFakeApprovedOrder(Guid orderId)
        {

           await  GenerateFakeAcceptedOrder(orderId);



            await TestHarness.Bus.Publish(
                    new OrderApprovedEvent
                    {
                        OrderId = orderId,
                    }
                );


            var instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Approved, TestHarness.TestTimeout);

            instance.Should().NotBeNull();
        }


        public async Task GenerateFakeFullfilledOrder(Guid orderId)
        {


            await GenerateFakeApprovedOrder(orderId);

            var instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Approved, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

            await TestHarness.Bus.Publish(new OrderFulfillmentCompletedEvent
            {
                OrderId = orderId,
                ShipmentId = Guid.NewGuid().ToString(),
            });

            instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Fullfilled, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

        }
        public async Task GenerateFakeCancelledOrder(Guid orderId)
        {
            await GenerateFakeSubmitedOrder(orderId);

            await TestHarness.Bus.Publish(new OrderCancelledEvent { OrderId = orderId, CancellationDate = DateTime.UtcNow, Reason = Guid.NewGuid().ToString() });

            var instance = await Repository.ShouldContainSagaInState(orderId, Machine, x => x.Cancelled, TestHarness.TestTimeout);

            instance.Should().NotBeNull();
        }

    }
}
