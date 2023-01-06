using FluentAssertions;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;
namespace MicroStore.Inventory.Application.Queries.Tests.Queries
{
    public class GetOrderWithExternalOrderIdQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_order_with_external_order_id()
        {
            var query = new GetOrderWithExternalIdQuery
            {
                ExternalOrderId = "23afe9ae-43c7-4522-bac8-363803e40612"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<OrderDto>().Result;

            result.ExternalOrderId.Should().Be(query.ExternalOrderId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_external_order_id_is_not_exist()
        {
            var query = new GetOrderWithExternalIdQuery
            {
                ExternalOrderId = Guid.NewGuid().ToString()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
