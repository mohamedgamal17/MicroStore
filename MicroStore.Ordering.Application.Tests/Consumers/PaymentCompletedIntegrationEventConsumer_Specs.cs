using MicroStore.Ordering.Events;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Ordering.Application.Tests.Consumers
{
    public class When_payment_accepted_integration_event_consumed : MassTransitTestFixture
    {

        [Test]
        public async Task Should_publish_order_payment_accepted_event()
        {
            await TestHarness.Bus.Publish(new PaymentCompletedIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                PaymentId = Guid.NewGuid().ToString(),
                PaymentCompletionDate = DateTime.Now
            });


            Assert.That(await TestHarness.Published.Any<OrderPaymentCompletedEvent>());
        }

    }
}
