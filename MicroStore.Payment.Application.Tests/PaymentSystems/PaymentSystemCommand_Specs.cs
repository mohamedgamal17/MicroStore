using FluentAssertions;
using MicroStore.Payment.Application.PaymentSystems;
using MicroStore.Payment.Domain;
using System.Net;

namespace MicroStore.Payment.Application.Tests.PaymentSystems
{
    public class When_receiving_update_payment_system_command : PaymentSystemCommandTestBase
    {
        [Test]
        public async Task Should_update_payment_system()
        {
            var fakeSystem = await GenerateFakePaymentSystem();

            var command = new UpdatePaymentSystemCommand
            {
                Name = fakeSystem.Name,
                IsEnabled = true
            };

            var result = await Send(command);

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            result.IsSuccess.Should().BeTrue();


            var system = await Find<PaymentSystem>(x => x.Name == fakeSystem.Name);

            system.IsEnabled.Should().BeTrue();
        }


        [Test]
        public async Task Should__return_error_result_with_status_code_404_while_payment_gateway_is_not_exist()
        {

            var result = await Send(new UpdatePaymentSystemCommand
            {
                Name = Guid.NewGuid().ToString(),
                IsEnabled = false
            });



            result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            result.IsFailure.Should().BeTrue();


        }


    }
}
