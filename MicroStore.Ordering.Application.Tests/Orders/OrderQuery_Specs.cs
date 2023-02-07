using FluentAssertions;
using MicroStore.Ordering.Application.Orders;
using System.Net;

namespace MicroStore.Ordering.Application.Tests.Orders
{
    public class When_receiving_get_order_list_query : BaseTestFixture
    {
        [Test]
        public async Task Should_get_order_paged_list()
        {
            var query = new GetOrderListQuery
            {
                PageNumber = 1,
                PageSize = 10,
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.PageNumber.Should().Be(query.PageNumber);
            result.PageSize.Should().Be(query.PageSize);

            result.Items.Count().Should().BeLessThanOrEqualTo(query.PageSize);

        }
    }


    public class When_receving_user_order_list_query : BaseTestFixture
    {
        [Test]
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

    public class When_receving_get_order_query : BaseTestFixture
    {
        [Test]
        public async Task Should_get_order_with_id()
        {
            var query = new GetOrderQuery
            {
                OrderId = Guid.Parse("5abe7f28-9e3c-40f5-a0cc-1ba1bffed3f9")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.OrderId);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_when_order_is_not_exist()
        {
            var query = new GetOrderQuery
            {
                OrderId = Guid.NewGuid()
            };

            var response = await Send(query);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }
    }
}
