using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Events;
namespace MicroStore.Ordering.Application.Tests.Consumers
{
    public class When_stock_confirmed_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_validated_event()
        {
            await TestHarness.Bus.Publish(new StockConfirmedIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString()
            });

            Assert.That(await TestHarness.Published.Any<OrderValidatedEvent>());
        }

    }
}
