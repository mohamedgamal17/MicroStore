using FluentAssertions;
using MassTransit.Testing;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Payment.IntegrationEvents;
namespace MicroStore.Ordering.Application.Tests.StateMachines
{
    public class CompleteOrder_Specs : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {

        private readonly Guid _fakeOrderId = Guid.NewGuid();

        private readonly string _orderNumber = Guid.NewGuid().ToString();

        private readonly string _paymentId = Guid.NewGuid().ToString();

        private readonly string _shippmentId = Guid.NewGuid().ToString();

        private readonly string _fakeUserId = Guid.NewGuid().ToString();

        [Test]
        public async Task Should_complete_order_to_end_succeful()
        {


            await TestHarness.Bus.Publish(
                    new OrderSubmitedEvent
                    {
                        OrderId = _fakeOrderId,
                        OrderNumber = _orderNumber,
                        BillingAddressId = Guid.NewGuid(),
                        ShippingAddressId = Guid.NewGuid(),
                        UserId = _fakeUserId,
                        SubmissionDate = DateTime.UtcNow,
                        OrderItems = GenerateFakeOrderItems()
                    }
                );

            var instance = await Repository.ShouldContainSagaInState(_fakeOrderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();


            await TestHarness.Bus.Publish(
                    new OrderApprovedEvent
                    {
                        OrderId = _fakeOrderId,
                        OrderNumber = _orderNumber
                    }
                );

            instance = await Repository.ShouldContainSagaInState(_fakeOrderId, Machine, x => x.Approved, TestHarness.TestTimeout);

            instance.Should().NotBeNull();


            await TestHarness.Bus.Publish(new OrderPaymentCreatedEvent
            {
                OrderId = _fakeOrderId,
                OrderNumber = _orderNumber,
                CustomerId = _fakeUserId,
                PaymentId = _paymentId
            });

            instance = await Repository.ShouldContainSagaInState(_fakeOrderId, Machine, x => x.Pending, TestHarness.TestTimeout);

            instance.Should().NotBeNull();


            await TestHarness.Bus.Publish(new OrderPaymentAcceptedEvent
            {
                OrderId = _fakeOrderId,
                OrderNubmer = _orderNumber,
                TransactionId = _paymentId,
                PaymentAcceptedDate = DateTime.UtcNow
            });

            instance = await Repository.ShouldContainSagaInState(_fakeOrderId, Machine, x => x.Processing, TestHarness.TestTimeout);

            instance.Should().NotBeNull();


            await TestHarness.Bus.Publish(new OrderCompletedEvent
            {
                OrderId = _fakeOrderId,
                ShippmentId = _shippmentId,
                OrderNumber = _orderNumber

            });

            instance = await Repository.ShouldContainSagaInState(_fakeOrderId, Machine, x => x.Completed, TestHarness.TestTimeout);
            instance.Should().NotBeNull();


            Assert.That(await TestHarness.Published.Any<AllocateOrderStockIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<CreatePaymentRequestIntegrationEvent>());

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
