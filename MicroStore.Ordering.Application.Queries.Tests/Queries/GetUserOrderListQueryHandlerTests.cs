using MicroStore.BuildingBlocks.Paging;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.Abstractions.Queries;
using FluentAssertions;
using System.Net;

namespace MicroStore.Ordering.Application.Queries.Tests.Queries
{
    public class GetUserOrderListQueryHandlerTests : BaseTestFixture
    {

        public async Task Should_get_user_order_list_paged()
        {
            var query = new GetUserOrderListQuery
            {
                UserId = "a84eb974-50b7-4ce6-95ac-515b54d2d7a3",
                PageNumber = 1,
                PageSize = 10
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);
            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);
            result.Items.All(x => x.UserId == query.UserId).Should().BeTrue();
        }
    }
}
