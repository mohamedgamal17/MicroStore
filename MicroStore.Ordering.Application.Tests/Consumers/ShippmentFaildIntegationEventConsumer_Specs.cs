//using MassTransit.Testing;
//using MicroStore.Ordering.Events;
//using MicroStore.Shipping.IntegrationEvent;

//namespace MicroStore.Ordering.Application.Tests.Consumers
//{
//    public class When_shippment_faild_integration_event_consumer : MassTransitTestFixture
//    {
//        [Test]
//        public async Task Should_publish_order_shippment_faild_event()
//        {
//            await TestHarness.Bus.Publish(new ShippmentFaildIntegrationEvent
//            {
//                OrderId = Guid.NewGuid(),
//                OrderNumber = Guid.NewGuid().ToString(),
//                ShippmentId = Guid.NewGuid().ToString(),
//                FaultReason = Guid.NewGuid().ToString(),
//                FaultDate = DateTime.UtcNow
//            });


//            Assert.That(await TestHarness.Published.Any<OrderShippmentFailedEvent>());
//        }

//    }
//}
