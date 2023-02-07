using MicroStore.Ordering.Application.StateMachines;
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

    [NonParallelizable]
    public class When_payment_faild_integration_event_consumed : MassTransitTestFixture
    {
        [Test]
        public async Task Should_publish_order_payment_rejected_event()
        {
            await TestHarness.Bus.Publish(new PaymentFaildIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                PaymentId = Guid.NewGuid().ToString(),
                FaultDate = DateTime.UtcNow,
                FaultReason = Guid.NewGuid().ToString(),
            });

            Assert.That(await TestHarness.Published.Any<OrderPaymentFaildEvent>());
        }

    }

}
