using FluentAssertions;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Domain;

namespace MicroStore.Payment.Application.Tests.Commands
{
    public class When_reciving_create_payment_request_command : BaseTestFixture
    {

        [Test]
        public async Task Should_create_payment_request()
        {
            CreatePaymentRequestCommand command = new CreatePaymentRequestCommand
            {
                OrderId = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                Amount = 50,
                CustomerId = Guid.NewGuid().ToString()
            };

            var result =  await Send(command);


            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.Id == result.PaymentId);

            paymentRequest.OrderId.Should().Be(command.OrderId);
            paymentRequest.OrderNumber.Should().Be(command.OrderNumber);
            paymentRequest.Amount.Should().Be(command.Amount);
            paymentRequest.CustomerId.Should().Be(command.CustomerId);
            paymentRequest.State.Should().Be(PaymentStatus.Created);
        }

    }
}
