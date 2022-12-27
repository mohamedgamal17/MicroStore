using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain;
using System.Net;
using Volo.Abp.Domain.Entities;
namespace MicroStore.Payment.Application.Tests.Commands
{
    public class ProcessPaymentRequestCommandHandler_Tests : BaseTestFixture
    {
        [Test]
        public async Task Should_begin_check_out_process()
        {

            PaymentRequest paymentRequest = await CreateFakePaymentRequest();

            var result = await Send(new ProcessPaymentRequestCommand
            {
                PaymentGatewayName = PaymentMethodConst.PaymentGatewayName,
                PaymentId = paymentRequest.Id,
                CancelUrl = Guid.NewGuid().ToString(),
                ReturnUrl = Guid.NewGuid().ToString()
            });

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);

            result.GetEnvelopeResult<PaymentProcessResultDto>().Result.CheckoutLink.Should().Be(PaymentMethodConst.CheckoutUrl);
        }


    

        [Test]
        public async Task Should__return_error_result_with_status_code_404_while_payment_gateway_is_not_exist()
        {
            PaymentRequest paymentRequest = await CreateFakePaymentRequest();

            var result = await Send(new ProcessPaymentRequestCommand
            {
                PaymentGatewayName = "NA",
                PaymentId = paymentRequest.Id,
                CancelUrl = Guid.NewGuid().ToString(),
                ReturnUrl = Guid.NewGuid().ToString()
            });


            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            result.IsFailure.Should().BeTrue();


        }

        [Test]
        public async Task Shoud_return_error_result_with_status_code_400_while_payment_gateway_is_not_active()
        {
            PaymentRequest paymentRequest = await CreateFakePaymentRequest();

            var result = await Send(new ProcessPaymentRequestCommand
            {
                PaymentGatewayName = PaymentMethodConst.NonActiveGateway,
                PaymentId = paymentRequest.Id,
                CancelUrl = Guid.NewGuid().ToString(),
                ReturnUrl = Guid.NewGuid().ToString()
            });


            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            result.IsFailure.Should().BeTrue();
        }


        [Test]

        public async Task Should_return_error_result_with_status_code_400_while_payment_state_is_not_waiting()
        {
            PaymentRequest paymentRequest = await CreateFakeCompletedPaymentRequest();
            var result = await Send(new ProcessPaymentRequestCommand
            {
                PaymentGatewayName = PaymentMethodConst.PaymentGatewayName,
                PaymentId = paymentRequest.Id,
                CancelUrl = Guid.NewGuid().ToString(),
                ReturnUrl = Guid.NewGuid().ToString()
            });

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
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


        private async Task<PaymentRequest> CreateFakeCompletedPaymentRequest()
        {
            var fakePaymentRequest = await CreateFakePaymentRequest();

            fakePaymentRequest.Complete(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), DateTime.UtcNow);

            return await Update(fakePaymentRequest);

        }
    }
}
