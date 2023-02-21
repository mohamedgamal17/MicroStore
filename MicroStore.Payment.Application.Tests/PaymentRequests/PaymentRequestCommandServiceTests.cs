using FluentAssertions;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain;
using MicroStore.Payment.Domain.Shared.Models;
namespace MicroStore.Payment.Application.Tests.PaymentRequests
{
    public class PaymentRequestCommandServiceTests : PaymentRequestCommandTestBase
    {
        private readonly IPaymentRequestCommandService _paymentRequestCommandService;

        public PaymentRequestCommandServiceTests()
        {
            _paymentRequestCommandService= GetRequiredService<IPaymentRequestCommandService>();
        }

        [Test]
        public async Task Should_create_payment_request()
        {
            var model = GeneratePaymentRequestModel();

            var result = await _paymentRequestCommandService.CreateAsync(model);

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.OrderId == model.OrderId);
            paymentRequest.OrderId.Should().Be(model.OrderId);
            paymentRequest.OrderNumber.Should().Be(model.OrderNumber);
            paymentRequest.TaxCost.Should().Be(model.TaxCost);
            paymentRequest.ShippingCost.Should().Be(model.ShippingCost);
            paymentRequest.TotalCost.Should().Be(model.TotalCost);
            paymentRequest.CustomerId.Should().Be(model.UserId);
            paymentRequest.State.Should().Be(PaymentStatus.Waiting);
        }

        [Test]
        public async Task Should_return_error_result_while_creating_payment_request_when_payment_request_for_order_is_already_created()
        {

            var fakePayment = await CreateFakePaymentRequest();

            var model = GeneratePaymentRequestModel();

            model.OrderId = fakePayment.OrderId;
            model.OrderNumber = fakePayment.OrderNumber;

            var result = await _paymentRequestCommandService.CreateAsync(model);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.BusinessLogicError);
        }

        [Test]
        public async Task Should_begin_check_out_process()
        {
            PaymentRequest paymentRequest = await CreateFakePaymentRequest();

            var result = await _paymentRequestCommandService.ProcessPaymentAsync(paymentRequest.Id, new ProcessPaymentRequestModel
            {
                GatewayName = PaymentMethodConst.PaymentGatewayName,
                CancelUrl = "http://cancel.com/",
                ReturnUrl = "http://success.com/",
            });
           
            result.IsSuccess.Should().BeTrue();

            result.Result.CheckoutLink.Should().Be(PaymentMethodConst.CheckoutUrl);
        }

        [Test]
        public async Task Should_return_error_result_while_processing_payment_request_when_payment_request_is_not_exist()
        {

            var result = await _paymentRequestCommandService.ProcessPaymentAsync(Guid.NewGuid().ToString(), new ProcessPaymentRequestModel
            {
                GatewayName = PaymentMethodConst.PaymentGatewayName,
                CancelUrl = "http://cancel.com/",
                ReturnUrl = "http://success.com/",
            });


            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }

        [Test]
        public async Task Should_return_error_result_while_processing_payment_request_when_payment_gateway_is_not_exist()
        {
            PaymentRequest paymentRequest = await CreateFakePaymentRequest();

            var result = await _paymentRequestCommandService.ProcessPaymentAsync(paymentRequest.Id, new ProcessPaymentRequestModel
            {
                GatewayName = "NA",
                CancelUrl = "http://cancel.com/",
                ReturnUrl = "http://success.com/",
            });


            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }


        [Test]
        public async Task Shoud_return_error_result_while_processing_payment_request_when_payment_gateway_is_not_active()
        {
            PaymentRequest paymentRequest = await CreateFakePaymentRequest();

            var result = await _paymentRequestCommandService.ProcessPaymentAsync(paymentRequest.Id, new ProcessPaymentRequestModel
            {
                GatewayName = PaymentMethodConst.NonActiveGateway,
                CancelUrl = "http://cancel.com/",
                ReturnUrl = "http://success.com/",
            });

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.BusinessLogicError);

            result.IsFailure.Should().BeTrue();
        }


        [Test]
        public async Task Should_return_error_result_while_processing_payment_request_when_payment_state_is_not_waiting()
        {
            PaymentRequest paymentRequest = await CreateFakeCompletedPaymentRequest();

            var result = await _paymentRequestCommandService.ProcessPaymentAsync(paymentRequest.Id, new ProcessPaymentRequestModel
            {
                GatewayName = PaymentMethodConst.PaymentGatewayName,
                CancelUrl = "http://cancel.com/",
                ReturnUrl = "http://success.com/",
            });

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.BusinessLogicError);
        }

        [Test]
        public async Task Should_refund_payment_request()
        {
            var fakePaymentRequest = await CreateFakeCompletedPaymentRequest();

            var result =  await _paymentRequestCommandService.RefundPaymentAsync(fakePaymentRequest.Id);

            result.IsSuccess.Should().BeTrue();

            var paymentRequest = await Find<PaymentRequest>(x=> x.Id== fakePaymentRequest.Id);

            paymentRequest.State.Should().Be(PaymentStatus.Refunded);
        }


        [Test]
        public async Task Should_return_error_result_while_refunding_payment_request_when_payment_request_is_not_exist()
        {
            var result = await _paymentRequestCommandService.RefundPaymentAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }

        [Test]
        public async Task Should_return_error_result_while_refunding_payment_request_when_payment_request_state_is_not_payed()
        {
            var fakePaymentRequest = await CreateFakePaymentRequest();

            var result = await _paymentRequestCommandService.RefundPaymentAsync(fakePaymentRequest.Id);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.BusinessLogicError);
        }

    }



}
