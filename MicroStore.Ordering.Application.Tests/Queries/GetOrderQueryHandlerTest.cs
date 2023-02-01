using FluentAssertions;
using MicroStore.Ordering.Application.Abstractions.Dtos;
using MicroStore.Ordering.Application.Abstractions.Queries;
using System.Net;
namespace MicroStore.Ordering.Application.Tests.Queries
{
    public class GetOrderQueryHandlerTest : BaseTestFixture
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
