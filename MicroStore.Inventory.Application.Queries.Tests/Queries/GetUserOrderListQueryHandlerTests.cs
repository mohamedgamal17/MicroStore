using FluentAssertions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;
namespace MicroStore.Inventory.Application.Queries.Tests.Queries
{
    public class GetUserOrderListQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_user_order_list_paged()
        {
            var query = new GetUserOrderListQuery
            {
                PageNumber = 1,
                PageSize = 3,
                UserId = "2cd94e7f-d80a-41c9-9805-75f1e3b4b925"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<PagedResult<OrderListDto>>().Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
        }
    }
}
