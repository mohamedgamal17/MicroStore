using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using System.Net;

namespace MicroStore.Shipping.Application.Tests.Queries
{
    public class GetShippingSystemWithNameQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_shipping_system_with_name()
        {
            var query = new GetShipmentSystemWithNameQuery
            {
                Name = "Example"
            };

            var respone = await Send(query);

            respone.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = respone.EnvelopeResult.Result;

            result.Name.Should().Be(query.Name);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_shipment_system_name_is_not_exist()
        {
            var query = new GetShipmentSystemWithNameQuery
            {
                Name = Guid.NewGuid().ToString()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
