using MicroStore.Ordering.Application.StateMachines;
using MicroStore.Ordering.IntegrationEvents;
using MicroStore.Ordering.IntegrationEvents.Models;
namespace MicroStore.Ordering.Application.Tests.Consumers
{
    [NonParallelizable]
    public class When_submited_order_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_submited_event()
        {
            await TestHarness.Bus.Publish(new SubmitOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                ShippingAddress = new AddressModel(),
                BillingAddress = new AddressModel(),
                SubmissionDate = DateTime.UtcNow,
                UserId = Guid.NewGuid().ToString(),
                OrderItems = new List<OrderItemModel>
                {
                    new OrderItemModel
                    {
                        Name = Guid.NewGuid().ToString(),
                        Sku = Guid.NewGuid().ToString(),
                        ExternalProductId =Guid.NewGuid().ToString(),
                        Quantity = 50,
                        UnitPrice = 50
                    }
                }
            });


            Assert.That(await TestHarness.Published.Any<OrderSubmitedEvent>());
        }
    }



    [NonParallelizable]
    public class When_fullfill_order_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_fullfillment_completed_event()
        {
            await TestHarness.Bus.Publish(new FullfillOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                ShipmentId = Guid.NewGuid().ToString(),

            });

            Assert.That(await TestHarness.Consumed.Any<FullfillOrderIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<OrderFulfillmentCompletedEvent>());
        }
    }

    [NonParallelizable]
    public class When_complete_order_integration_event_consumed : MassTransitTestFixture
    {

        [Test]
        public async Task Should_publish_order_completed_event()
        {
            await TestHarness.Bus.Publish(new CompleteOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                ShippedDate = DateTime.Now,
            });

            Assert.That(await TestHarness.Consumed.Any<CompleteOrderIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<OrderCompletedEvent>());

        }
    }
    [NonParallelizable]
    public class When_cancel_order_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_cancel_order_integration_event()
        {
            await TestHarness.Bus.Publish(new CancelOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                Reason = Guid.NewGuid().ToString(),
                CancellationDate = DateTime.Now,
            });

            Assert.That(await TestHarness.Consumed.Any<CancelOrderIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<OrderCancelledEvent>());
        }
    }
}
