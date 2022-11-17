//using FluentAssertions;
//using MassTransit.Testing;
//using MicroStore.Ordering.Application.StateMachines;
//using MicroStore.Ordering.Events;
//using MicroStore.Ordering.Events.Models;
//using MicroStore.Payment.IntegrationEvents;

//namespace MicroStore.Ordering.Application.Tests.StateMachines
//{
//    public class When_order_is_rejected : StateMachineTestFixture<OrderStateMachine, OrderStateEntity>
//    {
//        private readonly FakeOrderData _fakeOrderData = new FakeOrderData();
//        [Test]
//        public async Task Should_order_fail_when_order_is_rejected()
//        {
//            await TestHarness.Bus.Publish(
//                   new OrderSubmitedEvent
//                   {
//                       OrderId = _fakeOrderData.OrderId,
//                       OrderNumber = _fakeOrderData.OrderNumber,

//                       BillingAddressId = Guid.NewGuid(),
//                       ShippingAddressId = Guid.NewGuid(),
//                       UserId = _fakeOrderData.UserId,
//                       SubmissionDate = DateTime.UtcNow,
//                       OrderItems = new List<OrderItemModel>
//                       {

//                            new OrderItemModel
//                            {
//                                ItemName = "FakeName",
//                                ProductId = Guid.NewGuid(),
//                                Quantity = 5,
//                                UnitPrice = 50
//                            }
//                       }
//                   }
//               );

//            var instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Submitted, TestHarness.TestTimeout);

//            instance.Should().NotBeNull();

//            await TestHarness.Bus.Publish(
//                       new OrderOpenedEvent
//                       {
//                           OrderId = _fakeOrderData.OrderId,
//                           OrderNumber = _fakeOrderData.OrderNumber,
//                           TransactionId = _fakeOrderData.TransactionId
//                       }
//                );

//            instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Opened, TestHarness.TestTimeout);

//            instance.Should().NotBeNull();


//            await TestHarness.Bus.Publish(
//                    new OrderAcceptedEvent
//                    {
//                        OrderId = _fakeOrderData.OrderId,
//                        OrderNumber = _fakeOrderData.OrderNumber,
//                        AcceptedDate = DateTime.UtcNow
//                    }
//                );

//            instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Accepted, TestHarness.TestTimeout);

//            instance.Should().NotBeNull();


//            await TestHarness.Bus.Publish(new OrderRejectedEvent
//            {
//                OrderId = _fakeOrderData.OrderId,
//                OrderNumber = _fakeOrderData.OrderNumber,
//                RejectedDate = DateTime.UtcNow,
//                RejectReason = "FakeReason",
//                RejectedBy = Guid.NewGuid().ToString()
//            });

//            instance = await Repository.ShouldContainSagaInState(_fakeOrderData.OrderId, Machine, x => x.Rejected, TestHarness.TestTimeout);

//            instance.Should().NotBeNull();


//            Assert.That(await TestHarness.Published.Any<RefundUserIntegrationEvent>());

//        }

//    }
//}
