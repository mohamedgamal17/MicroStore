

using MicroStore.Payment.Api.Models;
using MicroStore.Payment.IntegrationEvents;
using MicroStore.Payment.IntegrationEvents.Responses;
using Moq;

namespace MicroStore.Payment.Api.Tests.Consumers
{
    public class When_create_payment_integration_event_recived : MassTransitTestFixture
    {


        [Test]
        public async Task Should_create_payment_and_respond_with_payment_created_response()
        {

            var clinet = TestHarness.GetRequestClient<CreatePaymentRequest>();

            await clinet.GetResponse<PaymentCreatedResponse>(new CreatePaymentRequest
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                TotalPrice = 500
            });

            Assert.That(await TestHarness.Consumed.Any<CreatePaymentRequest>());

            Assert.That(await TestHarness.Sent.Any<PaymentCreatedResponse>());

            MockedPaymentService.Verify(c => c.CreatePayment(It.IsAny<CreatePaymentModel>()), Times.Once);
        }
    }
}
