using FluentAssertions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;
namespace MicroStore.Payment.Application.Queries.Tests.Queries
{
    public class GetUserPaymentRequestListQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_user_payment_request_list_paged()
        {
            var query = new GetUserPaymentRequestListQuery
            {
                PageNumber = 1,
                PageSize = 3,
                UserId = "2cd94e7f-d80a-41c9-9805-75f1e3b4b925"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);

            result.Items.All(x => x.CustomerId == query.UserId).Should().BeTrue();
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task Should_get_user_payment_request_list_sorted_by_creation_date(bool desc)
        {
            var query = new GetUserPaymentRequestListQuery
            {
                PageNumber = 1,
                PageSize = 3,
                UserId = "2cd94e7f-d80a-41c9-9805-75f1e3b4b925",
                SortBy = "creation",
                Desc = desc
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            if (desc)
            {
                result.Items.Should().BeInDescendingOrder(x => x.CreationTime);
            }
            else
            {
                result.Items.Should().BeInAscendingOrder(x => x.CreationTime);
            }
        }
    }
}
