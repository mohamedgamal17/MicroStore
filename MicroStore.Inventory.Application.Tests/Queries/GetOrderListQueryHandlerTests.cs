using FluentAssertions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Inventory.Application.Tests.Queries
{
    public class GetOrderListQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_order_paged_list()
        {
            var query = new GetOrderListQuery
            {
                PageNumber = 1,
                PageSize = 3
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
