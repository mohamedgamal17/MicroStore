using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;
namespace MicroStore.Ordering.Application.Tests.Consumers
{
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
