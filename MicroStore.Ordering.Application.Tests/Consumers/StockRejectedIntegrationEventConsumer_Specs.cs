using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Events;
namespace MicroStore.Ordering.Application.Tests.Consumers
{
    public class When_stock_rejected_integration_event_consumed : MassTransitTestFixture
    {

        [Test]
        public async Task Should_publish_order_stock_rejected_event()
        {
            await TestHarness.Bus.Publish(new StockRejectedIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNubmer = Guid.NewGuid().ToString(),
                Details = "FakeDetails"
            });

            Assert.That(await TestHarness.Published.Any<OrderStockRejectedEvent>());
        }

    }
}
