using FluentAssertions;
using MicroStore.Catalog.Application.Abstractions.Products.Dtos;
using MicroStore.Catalog.Application.Abstractions.Products.Queries;
using System.Net;
namespace MicroStore.Catalog.Application.Queries.Tests.Queries
{
    public class GetProductQueryHandlerTests : BaseTestFixture
    {
        [Test]
        public async Task Should_get_product_with_id()
        {
            var query = new GetProductQuery
            {
                Id = Guid.Parse("94174b5b-25a8-4c29-9364-3482e9356231"),

            };

            var response = await Send(query);

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var result = response.GetEnvelopeResult<ProductDto>().Result;

            result.Id.Should().Be(query.Id);
        }

        [Test]
        public async Task Should_return_failure_result_with_status_code_404_not_found_when_product_is_not_exist()
        {
            var query = new GetProductQuery
            {
                Id = Guid.NewGuid(),
            };

            var response = await Send(query);

            response.IsFailure.Should().BeTrue();

            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


    }
}
