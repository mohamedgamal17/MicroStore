using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using System.Net;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Shipping.Application.Queries.Tests.Queries
{
    public class GetShippingSystemListQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_shipping_system_list()
        {
            var query = new GetShipmentSystemListQuery();

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Items.Count.Should().BeGreaterThan(0);
        }
    }
}
