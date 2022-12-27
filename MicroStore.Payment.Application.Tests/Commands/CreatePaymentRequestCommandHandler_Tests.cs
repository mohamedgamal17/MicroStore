using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Commands;
using MicroStore.Payment.Domain;
using System.Net;

namespace MicroStore.Payment.Application.Tests.Commands
{
    public class CreatePaymentRequestCommandHandler_Tests : BaseTestFixture
    {
        [Test]
        public async Task Should_create_payment_request()
        {
            CreatePaymentRequestCommand command = GeneratePaymentCreationCommand();

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.Created);

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

            result.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
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
