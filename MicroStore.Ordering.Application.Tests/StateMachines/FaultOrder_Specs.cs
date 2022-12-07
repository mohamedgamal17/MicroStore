using FluentAssertions;
using MassTransit.Testing;
using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.Events;
using MicroStore.Ordering.Events.Models;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Tests.StateMachines
{
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
                       BillingAddressId = Guid.NewGuid(),
                       ShippingAddressId = Guid.NewGuid(),
                       UserId = _fakeOrderData.UserId,
                       SubmissionDate = DateTime.UtcNow,
                       OrderItems = new List<OrderItemModel>
                       {
                            new OrderItemModel
                            {
                                ItemName = "FakeName",
                                ProductId = Guid.NewGuid(),
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

                       BillingAddressId = Guid.NewGuid(),
                       ShippingAddressId = Guid.NewGuid(),
                       UserId = _fakeOrderData.UserId,
                       SubmissionDate = DateTime.UtcNow,
                       OrderItems = new List<OrderItemModel>
                       {
                            new OrderItemModel
                            {
                                ItemName = "FakeName",
                                ProductId = Guid.NewGuid(),
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

                     BillingAddressId = Guid.NewGuid(),
                     ShippingAddressId = Guid.NewGuid(),
                     UserId = _fakeOrderData.UserId,
                     SubmissionDate = DateTime.UtcNow,
                     OrderItems = new List<OrderItemModel>
                     {
                            new OrderItemModel
                            {
                                ItemName = "FakeName",
                                ProductId = Guid.NewGuid(),
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
        }
    }


    //public class When_order_shippment_is_failed : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
    //{
    //    private readonly FakeOrderData _fakeOrderData = new FakeOrderData();


    //    [Test]
    //    public async Task Should_order_fail_when_shippment_is_failed()
    //    {
    //        await TestHarness.Bus.Publish(
    //              new OrderSubmitedEvent
    //              {
    //                  OrderId = _fakeOrderData.OrderId,
    //                  OrderNumber = _fakeOrderData.OrderNumber,

    //                  BillingAddressId = Guid.NewGuid(),
    //                  ShippingAddressId = Guid.NewGuid(),
    //                  UserId = _fakeOrderData.UserId,
    //                  SubmissionDate = DateTime.UtcNow,
    //                  OrderItems = new List<OrderItemModel>
    //                  {
    //                        new OrderItemModel
    //                        {
    //                            ItemName = "FakeName",
    //                            ProductId = Guid.NewGuid(),
    //                            Quantity = 5,
    //                            UnitPrice = 50
    //                        }
    //                  }
    //              }
    //          );

    //        var instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

    //        instance.Should().NotBeNull();


    //        await TestHarness.Bus.Publish(
    //             new OrderApprovedEvent
    //             {
    //                 OrderId = _fakeOrderData.OrderId,
    //                 OrderNumber = _fakeOrderData.OrderNumber
    //             }
    //        );

    //        instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Approved, TestHarness.TestTimeout);

    //        instance.Should().NotBeNull();

    //        await TestHarness.Bus.Publish(new OrderPaymentCreatedEvent
    //        {
    //            OrderId = _fakeOrderData.OrderId,
    //            CustomerId = _fakeOrderData.UserId,
    //            OrderNumber = _fakeOrderData.OrderNumber,
    //            PaymentId = _fakeOrderData.PaymentId
    //        });

    //        instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Pending, TestHarness.TestTimeout);

    //        instance.Should().NotBeNull();

    //        await TestHarness.Bus.Publish(new OrderPaymentAcceptedEvent
    //        {
    //            OrderId = _fakeOrderData.OrderId,
    //            OrderNubmer = _fakeOrderData.OrderNumber,
    //            TransactionId = _fakeOrderData.PaymentId,
    //            PaymentAcceptedDate = DateTime.UtcNow
    //        });

    //        instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Processing, TestHarness.TestTimeout);

    //        instance.Should().NotBeNull();



    //        await TestHarness.Bus.Publish(new OrderShippmentCreatedEvent
    //        {
    //            OrderId = _fakeOrderData.OrderId,
    //            ShippmentId = _fakeOrderData.ShippmentId,
    //            OrderNumber = _fakeOrderData.OrderNumber

    //        });

    //        instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Shipping, TestHarness.TestTimeout);

    //        instance.Should().NotBeNull();


    //        await TestHarness.Bus.Publish(new OrderShippmentFailedEvent
    //        {
    //            OrderId = _fakeOrderData.OrderId,
    //            ShippmentId = _fakeOrderData.ShippmentId,
    //            OrderNumber = _fakeOrderData.OrderNumber,
    //            FaultDate = DateTime.UtcNow,
    //            Reason = "FakeReason"
    //        });

    //        instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Faulted, TestHarness.TestTimeout);

    //        instance.Should().NotBeNull();


    //        Assert.That(await TestHarness.Published.Any<RefundPaymentIntegrationEvent>());
    //    }
    //}


    internal  class FakeOrderData
    {
        public readonly  Guid OrderId = Guid.NewGuid();

        public readonly  string OrderNumber = Guid.NewGuid().ToString();

        public readonly  string PaymentId = Guid.NewGuid().ToString();

        public readonly  string UserId = Guid.NewGuid().ToString();

        public readonly  string ShippmentId = Guid.NewGuid().ToString();

    }
}
