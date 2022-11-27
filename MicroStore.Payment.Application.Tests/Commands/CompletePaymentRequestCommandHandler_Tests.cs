using FluentAssertions;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Tests.Consts;
using MicroStore.Payment.Domain.Shared.Domain;
using MicroStore.Payment.IntegrationEvents;
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

            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.Id == fakePaymentRequest.Id);

            paymentRequest.PaymentGateway.Should().Be(PaymentMethodConst.PaymentGatewayName);

            paymentRequest.State.Should().Be(PaymentStatus.Payed);

            Assert.That(await TestHarness.Published.Any<PaymentAccepetedIntegrationEvent>());
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
