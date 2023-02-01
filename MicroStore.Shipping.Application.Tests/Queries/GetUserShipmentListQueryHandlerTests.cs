using FluentAssertions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Shipping.Application.Abstraction.Dtos;
using MicroStore.Shipping.Application.Abstraction.Queries;
using System.Net;

namespace MicroStore.Shipping.Application.Tests.Queries
{
    public class GetUserShipmentListQueryHandlerTests : BaseTestFixture
    {

        [Test]
        public async Task Should_get_user_shipment_list_paged()
        {
            var query = new GetUserShipmentListQuery
            {
                PageSize = 3,
                PageNumber = 1,
                UserId = "bfdd1deb-167d-4269-b0e3-351613b8a202"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);
            result.Items.Count().Should().BeLessThan(query.PageSize);
            result.Items.All(x => x.UserId == query.UserId).Should().BeTrue();
        }
    }
}
