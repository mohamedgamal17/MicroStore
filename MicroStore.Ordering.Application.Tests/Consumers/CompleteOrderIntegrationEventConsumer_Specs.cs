using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;

namespace MicroStore.Ordering.Application.Tests.Consumers
{
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
}
