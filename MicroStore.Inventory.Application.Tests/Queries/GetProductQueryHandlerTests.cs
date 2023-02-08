using FluentAssertions;
using MicroStore.Inventory.Application.Products;
using System.Net;
namespace MicroStore.Inventory.Application.Tests.Queries
{
    public class GetProductQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_product_with_id()
        {
            var query = new GetProductQuery
            {
                ProductId = Guid.Parse("6820be8e-0f4e-4ae2-94dc-e226a0e8f2f7")
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.EnvelopeResult.Result;

            result.Id.Should().Be(query.ProductId);
        }

        [Test]
        public async Task Should_return_status_code_404_notfound_when_product_id_is_not_exist()
        {
            var query = new GetProductQuery
            {
                ProductId = Guid.NewGuid()
            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
