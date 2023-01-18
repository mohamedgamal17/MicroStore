using MicroStore.Shipping.Application.Abstraction.Queries;
using FluentAssertions;
using System.Net;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Shipping.Application.Abstraction.Dtos;
namespace MicroStore.Shipping.Application.Queries.Tests.Queries
{
    public class GetShipmentListQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_shipment_list_paged()
        {
            var query = new GetShipmentListQuery
            {
                PageSize = 3,
                PageNumber = 1
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);
            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
        }     
    }
}
