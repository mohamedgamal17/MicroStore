using FluentAssertions;
using MicroStore.BuildingBlocks.Paging;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Payment.Application.Tests.Queries
{
    public class GetPaymentRequestListQueryHandlerTests : BaseTestFixture
    {

        [Test]
        public async Task Should_get_payment_request_list_paged()
        {
            var query = new GetPaymentRequestListQuery
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

        [TestCase(false)]
        [TestCase(true)]
        public async Task Should_get_payment_request_list_paged_sorted_by_creation_date(bool desc)
        {
            var query = new GetPaymentRequestListQuery
            {
                PageNumber = 1,
                PageSize = 3,
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
