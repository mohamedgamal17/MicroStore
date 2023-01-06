using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Payment.Application.Queries.Tests.Queries
{
    public class GetPaymentSystemWithNameQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_payment_system_with_name()
        {
            var query = new GetPaymentSystemWithNameQuery
            {
                SystemName = "Example"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<PaymentSystemDto>().Result;

            result.Name.Should().Be(query.SystemName);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_payment_system_name_is_not_exist()
        {
            var query = new GetPaymentSystemWithNameQuery
            {
                SystemName = Guid.NewGuid().ToString()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
