using FluentAssertions;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;
namespace MicroStore.Inventory.Application.Queries.Tests.Queries
{
    public class GetProductWithExternalIdQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_product_with_external_id()
        {
            var query = new GetProductWithExternalIdQuery
            {
                ExternalProductId = "159b39f4-d03d-48df-9c89-ef5aaba7ef52"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<ProductDto>().Result;

            result.ExternalProductId.Should().Be(query.ExternalProductId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_product_external_id_is_not_exist()
        {
            var query = new GetProductWithExternalIdQuery
            {
                ExternalProductId = Guid.NewGuid().ToString(),
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
