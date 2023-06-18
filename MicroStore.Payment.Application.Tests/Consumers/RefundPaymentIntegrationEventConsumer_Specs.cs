using FluentAssertions;
using MicroStore.Payment.Application.Domain;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.IntegrationEvents;
namespace MicroStore.Payment.Application.Tests.Consumers
{
    public class While_consuming_void_payment_integration_event : BaseTestFixture
    {

        [Test]
        public async Task Should_void_payment_request()
        {
            PaymentRequest fakepaymentRequest = await CreatePayedPaymentRequest();

            var voidPaymentIntegarationEvent = new RefundPaymentIntegrationEvent
            {
                PaymentId = fakepaymentRequest.Id.ToString(),
                OrderId = fakepaymentRequest.OrderId,
                UserId = fakepaymentRequest.UserId,
            };

            await TestHarness.Bus.Publish(voidPaymentIntegarationEvent);

            Assert.That(await TestHarness.Consumed.Any<RefundPaymentIntegrationEvent>());

            PaymentRequest paymentRequest = await SingleAsync<PaymentRequest>(x => x.Id == fakepaymentRequest.Id);

            paymentRequest.State.Should().Be(PaymentStatus.Refunded);

        }


        private async Task<PaymentRequest> CreatePayedPaymentRequest()
        {
            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                TotalCost = 50,
            };

            paymentRequest.Complete(PaymentMethodConst.PaymentGatewayName, Guid.NewGuid().ToString(), DateTime.UtcNow);


            await Insert(paymentRequest);

            return paymentRequest;
        }


    }
}
