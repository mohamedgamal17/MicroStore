using MicroStore.Payment.IntegrationEvents;
using Moq;

namespace MicroStore.Payment.Api.Tests.Consumers
{
    public class When_Accept_payment_integration_event_consumed : MassTransitTestFixture
    {


        [Test]
        public async Task Should_capture_payment_and_publish_payment_accepted_integration_event()
        {
            await TestHarness.Bus.Publish(new AcceptPaymentIntegationEvent 
            {
                TransactionId = Guid.NewGuid().ToString() 
            });


            Assert.That(await TestHarness.Consumed.Any<AcceptPaymentIntegationEvent>());

            Assert.That(await TestHarness.Published.Any<PaymentAccepetedIntegrationEvent>());

            MockedPaymentService.Verify(c=> c.CapturePayment(It.IsAny<string>()),Times.Once());
        }

        [Test]
        public async Task Should_refund_user_payment_and_publish_payment_rejected_when_any_error_happen()
        {
            MockedPaymentService.Setup(c => c.CapturePayment(It.IsAny<string>()))
                .Callback(() => throw new InvalidOperationException());

            await TestHarness.Bus.Publish(new AcceptPaymentIntegationEvent
            {
                TransactionId = Guid.NewGuid().ToString()
            });

            Assert.That(await TestHarness.Consumed.Any<AcceptPaymentIntegationEvent>());

            Assert.That(await TestHarness.Published.Any<PaymentRejectedIntegrationEvent>());

            MockedPaymentService.Verify(c => c.RefundPayment(It.IsAny<string>()), Times.Once);
        }



    }
}
