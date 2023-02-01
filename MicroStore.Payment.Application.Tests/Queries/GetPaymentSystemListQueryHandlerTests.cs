using FluentAssertions;
using MicroStore.Payment.Application.Abstractions.Dtos;
using MicroStore.Payment.Application.Abstractions.Queries;
using System.Net;
using Volo.Abp.Application.Dtos;

namespace MicroStore.Payment.Application.Tests.Queries
{
    public class GetPaymentSystemListQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_payment_system_list()
        {
            var query = new GetPaymentSystemListQuery();

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Items.Count.Should().BeGreaterThan(0);

        }
    }
}
