using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;

namespace MicroStore.Ordering.Application.Tests.Consumers
{
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
}
