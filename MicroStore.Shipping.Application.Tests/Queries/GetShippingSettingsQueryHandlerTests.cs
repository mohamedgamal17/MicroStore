using FluentAssertions;
using MicroStore.Shipping.Application.Abstraction.Queries;
using MicroStore.Shipping.Domain.Entities;
using System.Net;

namespace MicroStore.Shipping.Application.Tests.Queries
{
    public class GetShippingSettingsQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_retrive_shipping_settings()
        {
            var query = new GetShippingSettingsQuery();

            var result = await Send(query);

            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var response = result.EnvelopeResult;

            response.Should().NotBeNull();

        }
    }
}
