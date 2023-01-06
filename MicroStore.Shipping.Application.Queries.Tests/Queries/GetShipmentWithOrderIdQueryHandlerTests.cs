using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using System.Net;
namespace MicroStore.Shipping.Application.Queries.Tests.Queries
{
    public class GetShipmentWithOrderIdQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_shipment_with_order_id()
        {
            var query = new GetShipmentWithOrderIdQuery
            {
                OrderId = "379a9bf2-c85f-49c3-8422-6d6a10999bd6"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<ShipmentDto>().Result;

            result.OrderId.Should().Be(query.OrderId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_shipment_for_order_id_is_not_exist()
        {
            var query = new GetShipmentWithOrderIdQuery
            {
                OrderId = Guid.NewGuid().ToString()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
