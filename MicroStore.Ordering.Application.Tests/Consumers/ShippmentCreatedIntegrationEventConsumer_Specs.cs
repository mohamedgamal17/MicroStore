using MicroStore.Ordering.Events;
using MicroStore.Shipping.IntegrationEvent;
namespace MicroStore.Ordering.Application.Tests.Consumers
{
    public class When_shippment_created_integration_event_consumer : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_shippment_created_event()
        {
            await TestHarness.Bus.Publish(new ShippmentCreatedIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                ShippmentId = Guid.NewGuid().ToString()
            });

            Assert.That(await TestHarness.Published.Any<OrderShippmentCreatedEvent>());
        }
    }
}
