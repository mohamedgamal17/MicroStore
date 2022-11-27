//using MicroStore.Ordering.Events;
//using MicroStore.Shipping.IntegrationEvent;

//namespace MicroStore.Ordering.Application.Tests.Consumers
//{
//    public class When_shippment_completed_integration_event_consumer : MassTransitTestFixture
//    {
//        [Test]
//        public async Task Should_publish_order_shippment_completed()
//        {
//            await TestHarness.Bus.Publish(new ShippmentCompletedIntegrationEvent
//            {
//                OrderId = Guid.NewGuid(),
//                OrderNumber = Guid.NewGuid().ToString(),
//                ShippmentId = Guid.NewGuid().ToString(),
//                ShippedDate = DateTime.UtcNow
//            });

//            Assert.That(await TestHarness.Published.Any<OrderCompletedEvent>());
//        }


//    }
//}
