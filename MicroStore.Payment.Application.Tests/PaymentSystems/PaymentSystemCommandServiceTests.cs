using FluentAssertions;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Payment.Application.PaymentSystems;
using MicroStore.Payment.Domain;
namespace MicroStore.Payment.Application.Tests.PaymentSystems
{
    public class PaymentSystemCommandServiceTests : PaymentSystemCommandTestBase
    {
        private readonly IPaymentSystemCommandService _paymentSystemCommandService;

        public PaymentSystemCommandServiceTests()
        {
            _paymentSystemCommandService = GetRequiredService<IPaymentSystemCommandService>();
        }

        [Test]
        public async Task Should_update_payment_system()
        {
            var fakeSystem = await GenerateFakePaymentSystem();

            var result = await _paymentSystemCommandService.EnablePaymentSystemAsync(fakeSystem.Name, true);

            result.IsSuccess.Should().BeTrue();

            var system = await Find<PaymentSystem>(x => x.Name == fakeSystem.Name);

            system.IsEnabled.Should().BeTrue();
        }


        [Test]
        public async Task Should__return_error_result_with_status_code_404_while_payment_gateway_is_not_exist()
        {
            var result = await _paymentSystemCommandService.EnablePaymentSystemAsync(Guid.NewGuid().ToString(), true);

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }
    }
}
