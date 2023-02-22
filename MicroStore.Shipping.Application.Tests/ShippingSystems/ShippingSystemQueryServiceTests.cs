using FluentAssertions;
using MicroStore.BuildingBlocks.Results.Http;
using MicroStore.Shipping.Application.ShippingSystems;
using MicroStore.Shipping.Domain.Entities;
using System.Net;

namespace MicroStore.Shipping.Application.Tests.ShippingSystems
{
    public class ShippingSystemQueryServiceTests : BaseTestFixture
    {
        private readonly IShippingSystemQueryService _shippingSystemQueryService;

        public ShippingSystemQueryServiceTests()
        {
            _shippingSystemQueryService = GetRequiredService<IShippingSystemQueryService>();
        }

        [Test]
        public async Task Should_get_shipping_system_list()
        {  

            var result = await _shippingSystemQueryService.ListAsync();

            result.IsSuccess.Should().BeTrue();

            result.Result.Count.Should().BeGreaterThan(0);
        }


        [Test]
        public async Task Should_get_shipping_system_with_id()
        {
            string systemId = (await First<ShippingSystem>()).Id;

            var result = await _shippingSystemQueryService.GetAsync(systemId);

            result.IsSuccess.Should().BeTrue();

            result.Result.Id.Should().Be(systemId);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_shipping_system_by_id_when_shipment_system_is_not_exist()
        {

            var result = await _shippingSystemQueryService.GetAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }


        [Test]
        public async Task Should_get_shipping_system_with_name()
        {
            string sysName = "Example";

            var result = await _shippingSystemQueryService.GetByNameAsync(sysName);

            result.IsSuccess.Should().BeTrue();

            result.Result.Name.Should().Be(sysName);
        }

        [Test]
        public async Task Should_return_failure_result_while_getting_shipping_system_by_name_when_shipment_system_is_not_exist()
        {

            var result = await _shippingSystemQueryService.GetByNameAsync(Guid.NewGuid().ToString());

            result.IsFailure.Should().BeTrue();

            result.Error.Type.Should().Be(HttpErrorType.NotFoundError);
        }
    }
}
