using FluentAssertions;
using MassTransit.Testing;
using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Application.Models;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Payment.IntegrationEvents;
namespace MicroStore.Ordering.Application.Tests.StateMachines
{
    [NonParallelizable]
    public class When_order_stock_is_rejcted : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {
        private readonly FakeOrderData _fakeOrderData = new FakeOrderData();

        [Test]
        public async Task Should_order_fail_When_items_stock_is_rejected()
        {
            await TestHarness.Bus.Publish(
                   new OrderSubmitedEvent
                   {
                       OrderId = _fakeOrderData.OrderId,
                       OrderNumber = _fakeOrderData.OrderNumber,
                       BillingAddress = new AddressModel(),
                       ShippingAddress = new AddressModel(),
                       UserName = _fakeOrderData.UserId,
                       SubmissionDate = DateTime.UtcNow,
                       OrderItems = new List<OrderItemModel>
                       {
                            new OrderItemModel
                            {
                                Name = Guid.NewGuid().ToString(),
                                Sku = Guid.NewGuid().ToString(),
                                ExternalProductId = Guid.NewGuid().ToString(),
                                Quantity = 5,
                                UnitPrice = 50
                            }
                       }
                   }
               );

            var instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();


            await TestHarness.Bus.Publish(
              new OrderPaymentAcceptedEvent
              {
                  OrderId = _fakeOrderData.OrderId,
                  PaymentAcceptedDate = DateTime.UtcNow,
                  PaymentId = Guid.NewGuid().ToString(),
              }
          );

            instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Accepted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();


            await TestHarness.Bus.Publish(
                    new OrderStockRejectedEvent
                    {
                        OrderId = _fakeOrderData.OrderId,
                        Details = "FakeFaultReason"
                    }
                );

            instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Cancelled, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

            Assert.That(await TestHarness.Published.Any<RefundPaymentIntegrationEvent>());

        }

    }


    [NonParallelizable]
    public class When_order_payment_is_faulted : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {
        private readonly FakeOrderData _fakeOrderData = new FakeOrderData();

        [Test]
        public async Task Should_order_fail_when_payment_is_rejected()
        {

            await TestHarness.Bus.Publish(
                    new OrderSubmitedEvent
                    {
                        OrderId = _fakeOrderData.OrderId,
                        OrderNumber = _fakeOrderData.OrderNumber,
                        BillingAddress = new AddressModel(),
                        ShippingAddress = new AddressModel(),
                        UserName = _fakeOrderData.UserId,
                        SubmissionDate = DateTime.UtcNow,
                        OrderItems = new List<OrderItemModel>
                        {
                            new OrderItemModel
                            {
                                Name = Guid.NewGuid().ToString(),
                                Sku = Guid.NewGuid().ToString(),
                                ExternalProductId = Guid.NewGuid().ToString(),
                                Quantity = 5,
                                UnitPrice = 50
                            }
                        }
                    }
                );

            var instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

            await TestHarness.Bus.Publish(new OrderPaymentFaildEvent
            {
                OrderId = _fakeOrderData.OrderId,
                FaultDate = DateTime.UtcNow,
                FaultReason = "FakeReason"

            });


            instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Cancelled, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

        }
    }


    [NonParallelizable]
    public class When_order_is_cancelled : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    {
        private readonly FakeOrderData _fakeOrderData = new FakeOrderData();

        [Test]
        public async Task Should_cancel_order_and_publish_refund_payment_integration_event()
        {
            await TestHarness.Bus.Publish(
                    new OrderSubmitedEvent
                    {
                        OrderId = _fakeOrderData.OrderId,
                        OrderNumber = _fakeOrderData.OrderNumber,
                        BillingAddress = new AddressModel(),
                        ShippingAddress = new AddressModel(),
                        UserName = _fakeOrderData.UserId,
                        SubmissionDate = DateTime.UtcNow,
                        OrderItems = new List<OrderItemModel>
                        {
                            new OrderItemModel
                            {
                                Name = Guid.NewGuid().ToString(),
                                Sku = Guid.NewGuid().ToString(),
                                ExternalProductId = Guid.NewGuid().ToString(),
                                Quantity = 5,
                                UnitPrice = 50
                            }
                        }
                    }
                );

            var instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

            await TestHarness.Bus.Publish(
                    new OrderCancelledEvent
                    {
                        OrderId = _fakeOrderData.OrderId,
                        CancellationDate = DateTime.UtcNow,
                        Reason = Guid.NewGuid().ToString()
                    }
                );

            instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Cancelled, TestHarness.TestTimeout);

            instance.Should().NotBeNull();

            Assert.That(await TestHarness.Published.Any<RefundPaymentIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<ReleaseOrderStockIntegrationEvent>());
        }
    }
    internal  class FakeOrderData
    {
        public readonly  Guid OrderId = Guid.NewGuid();

        public readonly  string OrderNumber = Guid.NewGuid().ToString();

        public readonly  string PaymentId = Guid.NewGuid().ToString();

        public readonly  string UserId = Guid.NewGuid().ToString();

        public readonly  string ShippmentId = Guid.NewGuid().ToString();

    }
}
