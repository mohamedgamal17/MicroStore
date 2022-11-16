using FluentAssertions;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.IntegrationEvents;

namespace MicroStore.Payment.Application.Tests.Consumers
{
    public class While_consuming_void_payment_integration_event : BaseTestFixture
    {

        [Test]
        public async Task Should_void_payment_request()
        {
            PaymentRequest fakepaymentRequest = await Insert(new PaymentRequest(Guid.NewGuid(), "fakeordernumber", "fakecustomerId", 50));

            var voidPaymentIntegarationEvent = new VoidPaymentIntegrationEvent
            {
                PaymentId = fakepaymentRequest.Id.ToString(),
                OrderId = fakepaymentRequest.OrderId,
                CustomerId = fakepaymentRequest.CustomerId,
                OrderNumber = fakepaymentRequest.OrderNumber,
                FaultDate = DateTime.UtcNow
            };

            await TestHarness.Bus.Publish(voidPaymentIntegarationEvent);

            Assert.That(await TestHarness.Consumed.Any<VoidPaymentIntegrationEvent>());

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.Id == fakepaymentRequest.Id);

            paymentRequest.State.Should().Be(PaymentStatus.Void);
        }


    }
}
