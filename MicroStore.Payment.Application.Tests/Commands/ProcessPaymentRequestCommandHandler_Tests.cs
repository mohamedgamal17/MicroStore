using FluentAssertions;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain.Shared.Domain;
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

            result.CheckoutLink.Should().Be(PaymentMethodConst.CheckoutUrl);
        }


        [Test]
        public async Task Should_throw_entity_not_found_exception_while_payment_gateway_is_not_exist()
        {

            Func<Task> action = () => Send(new ProcessPaymentRequestCommand
            {
                PaymentGatewayName = "NA",
                PaymentId = Guid.NewGuid(),
                CancelUrl = Guid.NewGuid().ToString(),
                ReturnUrl = Guid.NewGuid().ToString()
            });

            await action.Should().ThrowExactlyAsync<EntityNotFoundException>();
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
