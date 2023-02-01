using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Payment.Application.Tests.Queries
{
    public class GetPaymentReqeuestQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_payment_request_with_id()
        {
            var query = new GetPaymentRequestQuery
            {
                PaymentRequestId = Guid.Parse("dd2be1f2-e980-40f2-a47d-b9194ef03fe7")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.PaymentRequestId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_payment_id_request_is_not_exist()
        {
            var query = new GetPaymentRequestQuery
            {
                PaymentRequestId = Guid.NewGuid()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

        }
    }
}
