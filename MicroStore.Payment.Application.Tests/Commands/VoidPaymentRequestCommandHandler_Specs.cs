using FluentAssertions;
using MicroStore.Payment.Application.Commands.Requests;
using MicroStore.Payment.Application.Domain;

namespace MicroStore.Payment.Application.Tests.Commands
{
    public class When_reciving_void_payment_command : BaseTestFixture
    {

        [Test]
        public async Task Should_void_pament_request()
        {

            PaymentRequest fakepaymentRequest = await Insert(new PaymentRequest(Guid.NewGuid(), "fakeordernumber", "fakecustomerId", 50));

            await Send(new VoidPaymentCommand
            {
                PaymentId = fakepaymentRequest.Id,
                FaultDate = DateTime.UtcNow
            });
          
            PaymentRequest paymentRequest = await Find<PaymentRequest>(x => x.Id == fakepaymentRequest.Id);

            paymentRequest.State.Should().Be(PaymentStatus.Void);
        }

    }
}
