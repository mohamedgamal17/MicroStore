using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Models;
using MicroStore.Payment.Domain;
using Stripe;
using Volo.Abp;
namespace MicroStore.Payment.Plugin.StripeGateway.Tests
{
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class StripePaymentMethod_Tests : BaseTestFixture
    {


        [Test]
        public async Task Should_create_checkout_session()
        {
            var paymentRequest = await GenerateWaitingPaymentRequest();

            var sut = GetRequiredService<StripePaymentMethod>();

            var model = new ProcessPaymentModel
            {
                CancelUrl = "https://cancel.com/",
                ReturnUrl = "https://success.com"
            };

            var result = await sut.Process(paymentRequest.Id, model);

            result.AmountTotal.Should().Be(paymentRequest.TotalCost);
            result.AmountSubTotal.Should().Be(paymentRequest.SubTotal);
            result.CancelUrl.Should().StartWith(model.CancelUrl);
            result.SuccessUrl.Should().StartWith(model.ReturnUrl);

            var session = await StripeSessionService.GetAsync(result.SessionId);

            session.Should().NotBeNull();

            session.ClientReferenceId.Should().Be(paymentRequest.Id.ToString());
        }

      

        [Test]
        public async Task Should_rethrow_stripe_exceptio_if_session_is_not_exist_while_completing_payment_request()
        {
            var model = new CompletePaymentModel
            {
                Token = Guid.NewGuid().ToString(),
            };

            var sut = GetRequiredService<StripePaymentMethod>();

            Func<Task> func = () => sut.Complete(model);

            await func.Should().ThrowExactlyAsync<StripeException>();
        }

        [Test]
        public async Task Should_throw_business_exception_while_checkout_session_is_not_payed()
        {
            var fakePaymentRequest = await GenerateWaitingPaymentRequest();

            var session = await GenerateUnPayedStripePaymentSessionRequest(fakePaymentRequest);

            var sut = GetRequiredService<StripePaymentMethod>();


            var model = new CompletePaymentModel
            {
                Token = session.Id,
            };

            Func<Task> func =()=> sut.Complete(model);
   
            await  func.Should().ThrowExactlyAsync<BusinessException>();
        }

        [Test]
        public async Task Should_refund_payment_request()
        {
            var fakePaymentRequest = await GeneratePayedPaymentRequest();

            var sut = GetRequiredService<StripePaymentMethod>();

            await sut.Refund(fakePaymentRequest.Id);

            PaymentRequest paymentRequest = await SingleAsync<PaymentRequest>(x => x.Id == fakePaymentRequest.Id);

            paymentRequest.State.Should().Be(PaymentStatus.Refunded);

        }

    }
}
