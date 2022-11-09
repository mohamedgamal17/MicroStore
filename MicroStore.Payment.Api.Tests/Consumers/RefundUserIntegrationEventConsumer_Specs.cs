using MicroStore.Payment.IntegrationEvents;
using Moq;

namespace MicroStore.Payment.Api.Tests.Consumers
{
    public class When_refund_user_integration_event_consumed : MassTransitTestFixture
    {

        public async Task Should_refund_user_payment()
        {
            await TestHarness.Bus.Publish(new RefundUserIntegrationEvent
            {
                TransactionId = Guid.NewGuid().ToString()
            });

            Assert.That(await TestHarness.Consumed.Any<RefundUserIntegrationEvent>());

            MockedPaymentService.Verify(c => c.RefundPayment(It.IsAny<string>()), Times.Once);
        }

    }
}
