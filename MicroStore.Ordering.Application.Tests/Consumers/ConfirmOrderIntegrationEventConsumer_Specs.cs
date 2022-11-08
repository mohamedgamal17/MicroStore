using MicroStore.Ordering.Events;
using MicroStore.Ordering.IntegrationEvents;
namespace MicroStore.Ordering.Application.Tests.Consumers
{
    public class When_confirm_order_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_confirmed_event()
        {
            await TestHarness.Bus.Publish(new ConfirmOrderIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                ConfirmationDate = DateTime.UtcNow
            });

            Assert.That(await TestHarness.Published.Any<OrderConfirmedEvent>());
        }
    }
}
