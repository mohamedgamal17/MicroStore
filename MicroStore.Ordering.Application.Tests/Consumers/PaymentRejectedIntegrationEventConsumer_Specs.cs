using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Tests.Consumers
{
    public class When_payment_rejected_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_payment_rejected_event()
        {
            await TestHarness.Bus.Publish(new PaymentRejectedIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                TransactionId = Guid.NewGuid().ToString(),
                FaultDate = DateTime.UtcNow
            });

            Assert.That(await TestHarness.Published.Any<OrderPaymentRejectedEvent>());
        }

    }
}
