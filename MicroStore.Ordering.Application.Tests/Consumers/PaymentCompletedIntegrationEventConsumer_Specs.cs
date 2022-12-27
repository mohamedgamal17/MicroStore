using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Tests.Consumers
{
    [NonParallelizable]
    public class When_payment_accepted_integration_event_consumed : MassTransitTestFixture
    {

        [Test]
        public async Task Should_publish_order_payment_accepted_event()
        {
            await TestHarness.Bus.Publish(new PaymentAccepetedIntegrationEvent
            {
                OrderId = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid().ToString(),
            });


            Assert.That(await TestHarness.Published.Any<OrderPaymentAcceptedEvent>());
        }

    }
}
