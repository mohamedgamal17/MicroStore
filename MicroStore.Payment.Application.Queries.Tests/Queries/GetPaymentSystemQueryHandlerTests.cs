using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Payment.Application.Queries.Tests.Queries
{
    public class GetPaymentSystemQueryHandlerTests :BaseTestFixture
    {
        [Test]
        public async Task Should_get_payment_system_with_id()
        {
            var query = new GetPaymentSystemQuery
            {
                SystemId = Guid.Parse("6cc93286-de57-4fe8-af64-90bdbe378e40")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.SystemId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_payment_system_id_is_not_exist()
        {
            var query = new GetPaymentSystemQuery
            {
                SystemId = Guid.NewGuid()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
