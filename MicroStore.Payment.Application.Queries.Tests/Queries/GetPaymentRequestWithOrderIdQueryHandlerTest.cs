using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Payment.Application.Queries.Tests.Queries
{
    public class GetPaymentRequestWithOrderIdQueryHandlerTest : BaseTestFixture
    {

        [Test]
        public async Task Should_get_payment_reqeust_with_order_id()
        {
            var query = new GetPaymentRequestWithOrderIdQuery
            {
                OrderId = "5abe7f28-9e3c-40f5-a0cc-1ba1bffed3f9"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.OrderId.Should().Be(query.OrderId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_payment_request_for_order_id_is_not_exist()
        {
            var query = new GetPaymentRequestWithOrderIdQuery
            {
                OrderId = Guid.NewGuid().ToString(),
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
