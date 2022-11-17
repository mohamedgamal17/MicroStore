using MicroStore.Ordering.Application.Consumers;
using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Tests.Consumers
{
    public class When_payment_faild_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_payment_rejected_event()
        {
            await TestHarness.Bus.Publish(new PaymentFaildIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid().ToString(),
                FaultDate = DateTime.UtcNow,
                FaultReason ="FakeReason"
            });

            Assert.That(await TestHarness.Published.Any<OrderPaymentFaildEvent>());
        }

    }
}
