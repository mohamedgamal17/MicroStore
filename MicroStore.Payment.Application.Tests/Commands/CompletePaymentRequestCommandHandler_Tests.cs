using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain;
using MicroStore.Payment.IntegrationEvents;
using System.Net;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Payment.Application.Tests.Commands
{
    public class CompletePaymentRequestCommandHandler_Tests : BaseTestFixture
    {
        [Test]
        public async Task Should_complete_payment_process()
        {

            PaymentRequest fakePaymentRequest = await CreateFakePaymentRequest();

            var result = await Send(new CompletePaymentRequestCommand
            {
                PaymentGatewayName = PaymentMethodConst.PaymentGatewayName,
                Token = fakePaymentRequest.Id.ToString(),
            });

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.Id == fakePaymentRequest.Id);

            paymentRequest.PaymentGateway.Should().Be(PaymentMethodConst.PaymentGatewayName);

            paymentRequest.State.Should().Be(PaymentStatus.Payed);

            Assert.That(await TestHarness.Published.Any<PaymentAccepetedIntegrationEvent>());
        }

        [Test]
        public async Task Should__return_error_result_with_status_code_404_while_payment_gateway_is_not_exist()
        {

            var result = await Send(new CompletePaymentRequestCommand
            {
                PaymentGatewayName = "NA",
                Token = Guid.NewGuid().ToString(),
       
            });


            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            result.IsFailure.Should().BeTrue();


        }

        [Test]
        public async Task Shoud_return_error_result_with_status_code_400_while_payment_gateway_is_not_active()
        {
            var result = await Send(new CompletePaymentRequestCommand
            {
                PaymentGatewayName = PaymentMethodConst.NonActiveGateway,
                Token = Guid.NewGuid().ToString(),

            });


            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            result.IsFailure.Should().BeTrue();
        }


        private Task<PaymentRequest> CreateFakePaymentRequest()
        {
            PaymentRequest paymentRequest = new PaymentRequest
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                TotalCost = 50,
            };


            return Insert(paymentRequest);
        }
    }
}
