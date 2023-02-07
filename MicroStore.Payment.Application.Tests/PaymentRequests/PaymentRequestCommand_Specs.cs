using FluentAssertions;
using MicroStore.Payment.Application.PaymentRequests;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain;
using MicroStore.Payment.IntegrationEvents;
using System.Net;

namespace MicroStore.Payment.Application.Tests.PaymentRequests
{
    public class When_receiving_create_payment_request_command : PaymentRequestCommandTestBase
    {
        [Test]
        public async Task Should_create_payment_request()
        {
            CreatePaymentRequestCommand command = GeneratePaymentCreationCommand();

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.IsSuccess.Should().BeTrue();

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.OrderId == command.OrderId);

            paymentRequest.OrderId.Should().Be(command.OrderId);
            paymentRequest.OrderNumber.Should().Be(command.OrderNumber);
            paymentRequest.TaxCost.Should().Be(command.TaxCost);
            paymentRequest.ShippingCost.Should().Be(command.ShippingCost);
            paymentRequest.TotalCost.Should().Be(command.TotalCost);
            paymentRequest.CustomerId.Should().Be(command.UserId);
            paymentRequest.State.Should().Be(PaymentStatus.Waiting);
        }

        [Test]
        public async Task Should_return_error_result_with_status_code_400_while_payment_request_for_order_is_already_created()
        {

            var fakePayment = await CreateFakePaymentRequest();

            CreatePaymentRequestCommand command = GeneratePaymentCreationCommand();
            command.OrderId = fakePayment.OrderId;
            command.OrderNumber = fakePayment.OrderNumber;


            var result = await Send(command);

            result.IsFailure.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        private CreatePaymentRequestCommand GeneratePaymentCreationCommand()
        {
            return new CreatePaymentRequestCommand
            {
                OrderId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                SubTotal = 50,
                TotalCost = 50,
                Items = new List<OrderItemModel>
                {
                    new OrderItemModel
                    {
                        Name = Guid.NewGuid().ToString(),
                        ProductId = Guid.NewGuid().ToString(),
                        Sku = Guid.NewGuid().ToString(),
                        Image = Guid.NewGuid().ToString(),
                        Quantity = 5,
                        UnitPrice = 50
                    }
                }
            };
        }
    }

    public class When_receiving_process_payment_request_command : PaymentRequestCommandTestBase
    {
        [Test]
        public async Task Should_begin_check_out_process()
        {

            PaymentRequest paymentRequest = await CreateFakePaymentRequest();

            var result = await Send(new ProcessPaymentRequestCommand
            {
                PaymentGatewayName = PaymentMethodConst.PaymentGatewayName,
                PaymentId = paymentRequest.Id,
                CancelUrl = "http://cancel.com/",
                ReturnUrl = "http://success.com/",
            });

            result.IsSuccess.Should().BeTrue();

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.EnvelopeResult.Result.CheckoutLink.Should().Be(PaymentMethodConst.CheckoutUrl);
        }




        [Test]
        public async Task Should__return_error_result_with_status_code_404_while_payment_gateway_is_not_exist()
        {
            PaymentRequest paymentRequest = await CreateFakePaymentRequest();

            var result = await Send(new ProcessPaymentRequestCommand
            {
                PaymentGatewayName = "NA",
                PaymentId = paymentRequest.Id,
                CancelUrl = "http://cancel.com/",
                ReturnUrl = "http://success.com/",
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
    }


    public class When_receiving_complete_payment_request_command : PaymentRequestCommandTestBase
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

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

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

    }
}
