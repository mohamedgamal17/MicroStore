using FluentAssertions;
using MicroStore.Inventory.Application.Abstractions.Dtos;
using MicroStore.Inventory.Application.Abstractions.Queries;
using System.Net;

namespace MicroStore.Inventory.Application.Queries.Tests.Queries
{
    public class GetProductWithSkuQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_product_with_sku()
        {
            var query = new GetProductWithSkuQuery
            {
                Sku = "IPHONE-9"
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Sku.Should().Be(query.Sku);

        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_product_sku_is_not_exist()
        {
            var query = new GetProductWithSkuQuery
            {
                Sku = Guid.NewGuid().ToString()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
