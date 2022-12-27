using MicroStore.Inventory.IntegrationEvents;
using MicroStore.Ordering.Events;
namespace MicroStore.Ordering.Application.Tests.Consumers
{
    [NonParallelizable]
    public class When_stock_rejected_integration_event_consumed : MassTransitTestFixture
    {

        [Test]
        public async Task Should_publish_order_stock_rejected_event()
        {
            await TestHarness.Bus.Publish(new StockRejectedIntegrationEvent
            {
                ExternalOrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                Details = Guid.NewGuid().ToString(),
            });

            Assert.That(await TestHarness.Published.Any<OrderStockRejectedEvent>());
        }

    }
}
