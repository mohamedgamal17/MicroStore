using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Models;
using MicroStore.Payment.Domain;
using Stripe;
using System.Net;
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

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var response = result.EnvelopeResult.Result;

            response.AmountTotal.Should().Be(paymentRequest.TotalCost);
            response.AmountSubTotal.Should().Be(paymentRequest.SubTotal);
            response.CancelUrl.Should().StartWith(model.CancelUrl);
            response.SuccessUrl.Should().StartWith(model.ReturnUrl);

            var session = await StripeSessionService.GetAsync(response.SessionId);

            session.Should().NotBeNull();

            session.ClientReferenceId.Should().Be(paymentRequest.Id.ToString());
        }

      

        [Test]
        public async Task Should_return_failure_result_if_session_is_not_exist_while_completing_payment_request()
        {
            var model = new CompletePaymentModel
            {
                Token = Guid.NewGuid().ToString(),
            };

            var sut = GetRequiredService<StripePaymentMethod>();

            var result = await sut.Complete(model);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Should_return_failure_result_while_checkout_session_is_not_payed()
        {
            var fakePaymentRequest = await GenerateWaitingPaymentRequest();

            var session = await GenerateUnPayedStripePaymentSessionRequest(fakePaymentRequest);

            var sut = GetRequiredService<StripePaymentMethod>();


            var model = new CompletePaymentModel
            {
                Token = session.Id,
            };

            var result  = await sut.Complete(model);

            result.IsFailure.Should().BeTrue();
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
