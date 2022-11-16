using FluentAssertions;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Payment.Application.Tests.Consumers
{
    public class When_consuming_create_payment_request_integration_event : BaseTestFixture
    {

        [Test]
        public async Task Should_create_payment_request_and_publish_payment_created_integation_event()
        {
            CreatePaymentIntegrationEvent createPaymentIntegrationEvent = new CreatePaymentIntegrationEvent
            {
                OrderId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                TotalPrice = 50
            };

            await TestHarness.Bus.Publish(createPaymentIntegrationEvent);

            Assert.That(await TestHarness.Consumed.Any<CreatePaymentIntegrationEvent>());
            Assert.That(await TestHarness.Published.Any<CreatePaymentIntegrationEvent>());

            long paymentCount  = await Count<PaymentRequest>();

            paymentCount.Should().Be(1);
        }


    }
}
