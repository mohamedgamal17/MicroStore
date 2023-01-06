using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using System.Net;

namespace MicroStore.Shipping.Application.Queries.Tests.Queries
{
    public class GetShipmentQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_shipment_with_id()
        {
            var query = new GetShipmentQuery
            {
                ShipmentId = Guid.Parse("70b35252-5149-4418-b6bb-c647ea3cc030")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<ShipmentDto>().Result;

            result.ShipmentId.Should().Be(result.ShipmentId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_Shipment_id_is_not_exist()
        {
            var query = new GetShipmentQuery
            {
                ShipmentId = Guid.NewGuid()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

    }
}
