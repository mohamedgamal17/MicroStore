using FluentAssertions;
using MicroStore.Payment.Application.PaymentSystems;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace MicroStore.Payment.Application.Tests.PaymentSystems
{
    public class PaymentSystemQueryServiceTests : BaseTestFixture
    {
        private readonly IPaymentSystemQueryService _paymentSystemQueryService;

        public PaymentSystemQueryServiceTests()
        {
            _paymentSystemQueryService = GetRequiredService<IPaymentSystemQueryService>();
        }

        [Test]
        public async Task Should_get_payment_system_list()
        {
            var result = await _paymentSystemQueryService.ListPaymentSystemAsync();

            result.Value.Count.Should().BeGreaterThan(0);

        }

        [Test]
        public async Task Should_get_payment_system_with_id()
        {
            string systemId = "6cc93286-de57-4fe8-af64-90bdbe378e40";

            var result = await _paymentSystemQueryService.GetAsync(systemId);

            result.IsSuccess.Should().BeTrue();

            result.Value.Id.Should().Be(systemId);
        }

        [Test]
        public async Task Should_return_error_result_while_getting_payment_system_by_id_when_payment_system_is_not_exist()
        {

            var result = await _paymentSystemQueryService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Test]
        public async Task Should_get_payment_system_with_name()
        {
            string sysName = "Example";

            var result = await _paymentSystemQueryService.GetBySystemNameAsync(sysName);

            result.IsSuccess.Should().BeTrue();

            result.Value.Name.Should().Be(sysName);
        }

        [Test]
        public async Task Should_return_error_result_while_getting_payment_system_by_name_when_payment_system_is_not_exist()
        {

            var result = await _paymentSystemQueryService.GetBySystemNameAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Exception.Should().BeOfType<EntityNotFoundException>();
        }
    }
}
